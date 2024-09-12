using GeneralEntries.Helpers.Data_Structure;
using GeneralEntries.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GeneralEntries.ContextClass;

public class DbContextClass : IdentityDbContext<ApplicationUser>
{
    public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
    {
        try
        {
            var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (databaseCreator != null)
            {
                if (!databaseCreator.CanConnect()) databaseCreator.Create();
                if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        var IdentityRole1 = new IdentityRole()
        {
            Id = Guid.NewGuid().ToString(),
            Name = ConstantClass.ROLE_ADMIN,
            NormalizedName = ConstantClass.ROLE_ADMIN.ToUpper(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        builder.Entity<IdentityRole>().HasData(IdentityRole1);

        var IdentityRole2 = new IdentityRole()
        {
            Id = Guid.NewGuid().ToString(),
            Name = ConstantClass.ROLE_MANAGER,
            NormalizedName = ConstantClass.ROLE_MANAGER.ToUpper(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        builder.Entity<IdentityRole>().HasData(IdentityRole2);

        var IdentityRole3 = new IdentityRole()
        {
            Id = Guid.NewGuid().ToString(),
            Name = ConstantClass.ROLE_EMPLOYEE,
            NormalizedName = ConstantClass.ROLE_EMPLOYEE.ToUpper(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        builder.Entity<IdentityRole>().HasData(IdentityRole3);

        var IdentityRole4 = new IdentityRole()
        {
            Id = Guid.NewGuid().ToString(),
            Name = ConstantClass.ROLE_USER,
            NormalizedName = ConstantClass.ROLE_USER.ToUpper(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        builder.Entity<IdentityRole>().HasData(IdentityRole4);

        var AdminUser = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Abdul Mussawir",
            Email = "Abdul_mussawir@hotmail.com",
            FirstName = "AbdulMussawir",
            LastName = "Sheikh",
            NormalizedEmail = "Abdul_mussawir@hotmail.com".ToUpper(),
            NormalizedUserName = "AbdulMussawir".ToUpper(),
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        AdminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(AdminUser, "Saba123");
        builder.Entity<ApplicationUser>().HasData(AdminUser);

        var ManagerUser = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Raheel",
            Email = "Raheel@hotmail.com",
            FirstName = "Raheel",
            LastName = "Sheikh",
            NormalizedEmail = "Raheel@hotmail.com".ToUpper(),
            NormalizedUserName = "Raheel".ToUpper(),
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        ManagerUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(ManagerUser, "Saba123");
        builder.Entity<ApplicationUser>().HasData(ManagerUser);

        var EmployeeUser = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Shehneela Khan",
            Email = "Shehneela@hotmail.com",
            FirstName = "Shehneela",
            LastName = "Khan",
            NormalizedEmail = "Shehneela@hotmail.com".ToUpper(),
            NormalizedUserName = "Shehneela".ToUpper(),
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        EmployeeUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(EmployeeUser, "Saba123");
        builder.Entity<ApplicationUser>().HasData(EmployeeUser);

        var NormalUser = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Salman Khan",
            Email = "Salman@hotmail.com",
            FirstName = "Salman",
            LastName = "Khan",
            NormalizedEmail = "Salman@hotmail.com".ToUpper(),
            NormalizedUserName = "Salman".ToUpper(),
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };
        NormalUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(NormalUser, "Saba123");
        builder.Entity<ApplicationUser>().HasData(NormalUser);

        builder.Entity<Employee>()
            //.ToTable("Employee", x=>x.IsTemporal())
            //    .Ignore(x=>x.UserName)
            .HasData(new Employee
            {
                EmployeeId = 1,
                EmployeeName = "Raheel",
                Age = 29,
                Salary = 100000,
                ApplicationUserId = AdminUser.Id.ToString()
            });

        builder.Entity<IdentityUserRole<string>>().HasData
         (
            new IdentityUserRole<string>() { UserId = AdminUser.Id.ToString(), RoleId = IdentityRole1.Id.ToString() },
            new IdentityUserRole<string>() { UserId = ManagerUser.Id.ToString(), RoleId = IdentityRole2.Id.ToString() },
            new IdentityUserRole<string>() { UserId = EmployeeUser.Id.ToString(), RoleId = IdentityRole3.Id.ToString() },
             new IdentityUserRole<string>() { UserId = NormalUser.Id.ToString(), RoleId = IdentityRole4.Id.ToString() }
         );


        builder.Entity<IdentityUserClaim<string>>().HasData(
            new IdentityUserClaim<string>() { Id = 1, UserId = AdminUser.Id.ToString(), ClaimType = ConstantClass.ROLE_ADMIN, ClaimValue = ConstantClass.ADD_MANAGER_CALIM_VALUE },
            new IdentityUserClaim<string>() { Id = 2, UserId = AdminUser.Id.ToString(), ClaimType = ConstantClass.ROLE_ADMIN, ClaimValue = ConstantClass.EDIT_MANAGER_CALIM_VALUE },
            new IdentityUserClaim<string>() { Id = 3, UserId = AdminUser.Id.ToString(), ClaimType = ConstantClass.ROLE_ADMIN, ClaimValue = ConstantClass.DELETE_MANAGER_CALIM_VALUE },
            new IdentityUserClaim<string>() { Id = 4, UserId = AdminUser.Id.ToString(), ClaimType = ConstantClass.ROLE_ADMIN, ClaimValue = ConstantClass.GET_MANAGER_CALIM_VALUE },

            new IdentityUserClaim<string>() { Id = 5, UserId = ManagerUser.Id.ToString(), ClaimType = ConstantClass.ROLE_MANAGER, ClaimValue = ConstantClass.ADD_EMPLOYEE_CALIM_VALUE },
            new IdentityUserClaim<string>() { Id = 6, UserId = ManagerUser.Id.ToString(), ClaimType = ConstantClass.ROLE_MANAGER, ClaimValue = ConstantClass.EDIT_EMPLOYEE_CALIM_VALUE },
            new IdentityUserClaim<string>() { Id = 7, UserId = ManagerUser.Id.ToString(), ClaimType = ConstantClass.ROLE_MANAGER, ClaimValue = ConstantClass.DELETE_EMPLOYEE_CALIM_VALUE },
            new IdentityUserClaim<string>() { Id = 8, UserId = ManagerUser.Id.ToString(), ClaimType = ConstantClass.ROLE_MANAGER, ClaimValue = ConstantClass.GET_EMPLOYEE_CALIM_VALUE }

            );
        base.OnModelCreating(builder);
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<ChartsofAccounts> ChartsOfAccounts => Set<ChartsofAccounts>();
    public DbSet<GeneralEntry> GeneralEntries => Set<GeneralEntry>();
    public DbSet<GeneralVoucher> GeneralVouchers => Set<GeneralVoucher>();
}
