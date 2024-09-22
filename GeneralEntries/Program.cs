using GeneralEntries.Models;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text;
using GeneralEntries.Global;
using GeneralEntries.ContextClass;
using GeneralEntries.Helpers.Data_Structure;
using System.Threading.RateLimiting;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using GeneralEntries.RepositoryLayer.ServiceClass;
using Microsoft.AspNetCore.Mvc.Versioning;
using NLog;
using NLog.Web;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Serilog;
using Serilog.Formatting.Json;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    ConfigurationManager Configuration = builder.Configuration; // allows both to access and to set up the config
    IWebHostEnvironment env = builder.Environment;

    //var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "applogs-.txt");

    //Log.Logger = new LoggerConfiguration()
    //    .MinimumLevel.Information() // Use Information level for production
    //    .Enrich.FromLogContext()
    //    .WriteTo.Console()
    //    .WriteTo.File(new JsonFormatter(),
    //                  logFilePath,
    //                  rollingInterval: RollingInterval.Day,
    //                  retainedFileCountLimit: 7,
    //                  fileSizeLimitBytes: 10 * 1024 * 1024,
    //                  shared: true)
    //    .CreateLogger();

    Log.Information("Application starting...");

    builder.Host.SerilogConfiguration();

    builder.Services.AddDbContext<DbContextClass>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("NewConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });
    });

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetSection("RedisConnection").GetValue<string>("LocalHost");
        options.InstanceName = builder.Configuration.GetSection("RedisConnection").GetValue<string>("InstanceName");
    });

    builder.Services.AddScoped<IAuthLayer, AuthLayer>(); // If stateless, use Singleton
    builder.Services.AddScoped<IEmployeeLayer, EmployeeLayer>(); // Same here if stateless
    builder.Services.AddScoped<ICompanyLayer, CompanyLayer>();

    builder.Services.AddResponseCaching();
    builder.Services.AddHttpClient();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddControllers(options =>
    {
        //options.Filters.Add(typeof(ExceptionFilterGlobally));
        options.Filters.Add<ExceptionFilterGlobally>(); // Use generic type

    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    //builder.Services.AddHangfire((sp, config) =>
    //{
    //    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("NewConnection");
    //    config.UseSqlServerStorage(connectionString);
    //});
    //builder.Services.AddHangfireServer();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;

    }).AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<DbContextClass>()
                    .AddDefaultTokenProviders();

    builder.Services.AddTransient<GlobalExceptionHandler>();

    TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    builder.Services.AddSingleton(new MapsterProfile());

    builder.Services.AddApiVersioning(o =>
    {
        o.AssumeDefaultVersionWhenUnspecified = false;
        o.ApiVersionReader = new UrlSegmentApiVersionReader();
        o.ReportApiVersions = true;
    });

    builder.Services.AddCors(cors => cors.AddPolicy("AllowApi", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }));

    builder.Services.AddRateLimiter(rateLimiterOptions =>
    {
        rateLimiterOptions.AddTokenBucketLimiter("token", options =>
        {
            options.TokenLimit = 100;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 5;
            options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
            options.TokensPerPeriod = 20;
            options.AutoReplenishment = true;
        });
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdminClaims", policy =>
        {
            policy.RequireClaim("Admin", "Get Manager")
                      .RequireRole("Admin");
        });
    });

    builder.Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        var key = Encoding.ASCII.GetBytes(Configuration["JWTKey:Secret"]);

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateAudience = true,
            ValidIssuer = Configuration["JWTKey:ValidIssuer"],
            ValidAudience = Configuration["JWTKey:ValidAudience"],
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });


    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: Bearer eyJhbGciOiJIUzI1",
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
           {
                 new OpenApiSecurityScheme
                  {
                       Reference = new OpenApiReference
                        {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                        }
                  },
                  new string[]{}
           }
     });
    });

    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseResponseCaching();
    app.UseCors("AllowApi");
    app.ConfigureExceptionHandler(logger);
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRateLimiter();
    app.UseMiddleware<GlobalExceptionHandler>();

    app.MapControllers();

    //app.UseHangfireDashboard("/test/job-dashboard", new DashboardOptions
    //{
    //    DashboardTitle = "Hangfire Job Demo Application",
    //    DarkModeEnabled = false,
    //    DisplayStorageConnectionString = false,
    //    Authorization = new[]
    //{
    //    new HangfireCustomBasicAuthenticationFilter
    //    {
    //        User = "admin",
    //        Pass = "admin123"
    //    }
    //}
    //});

    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception.Message, $"Stopped program because of exception - {DateTime.Now}");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

