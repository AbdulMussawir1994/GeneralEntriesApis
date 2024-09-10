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

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager Configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment env = builder.Environment;

var logger = LoggerFactory
    .Create(config =>
    {
        config.AddConsole();
    }).CreateLogger("Program");

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddResponseCaching();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilterGlobally));

}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<DbContextClass>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NewConnection"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    });
});

//builder.Services.AddDatabase(Configuration
//    .GetConnectionString("NewConnection"));

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

builder.Services.AddScoped<IAuthLayer, AuthLayer>();

builder.Services.AddTransient<GlobalExceptionHandler>();

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton(new MapsterProfile());

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

app.UseRateLimiter();

app.Logger.LogInformation("Adding Swagger...");

app.UseHttpsRedirection();

app.UseCors("AllowApi");

app.UseResponseCaching();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandler>();

app.MapControllers();

app.Run();
