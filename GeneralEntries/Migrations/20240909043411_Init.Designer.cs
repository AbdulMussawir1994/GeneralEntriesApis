﻿// <auto-generated />
using System;
using GeneralEntries.ContextClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GeneralEntries.Migrations
{
    [DbContext(typeof(DbContextClass))]
    [Migration("20240909043411_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GeneralEntries.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "fac60314-3509-4de3-a9dc-6fa1d6526016",
                            DateCreated = new DateTime(2024, 9, 9, 4, 34, 2, 940, DateTimeKind.Utc).AddTicks(7408),
                            DateModified = new DateTime(2024, 9, 9, 4, 34, 2, 940, DateTimeKind.Utc).AddTicks(7409),
                            Email = "Abdul_mussawir@hotmail.com",
                            EmailConfirmed = false,
                            FirstName = "AbdulMussawir",
                            LastName = "Sheikh",
                            LockoutEnabled = false,
                            NormalizedEmail = "ABDUL_MUSSAWIR@HOTMAIL.COM",
                            NormalizedUserName = "ABDULMUSSAWIR",
                            PasswordHash = "AQAAAAIAAYagAAAAEMYTaZIyZEz9Ho7BniOx7sxcM6kEP7H+t8IqoF97q7d75I8xrbkgvIULZUQI61reXg==",
                            PhoneNumberConfirmed = false,
                            RefreshToken = "",
                            RefreshTokenExpiryTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SecurityStamp = "5f1d0701-26f5-4e49-a421-798fc340e966",
                            TwoFactorEnabled = false,
                            UserName = "Abdul Mussawir"
                        },
                        new
                        {
                            Id = "77b5e980-9439-4d81-922a-b368d1bf3ecf",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "4502a6cf-7f54-4c26-9d68-158471c51909",
                            DateCreated = new DateTime(2024, 9, 9, 4, 34, 3, 2, DateTimeKind.Utc).AddTicks(6633),
                            DateModified = new DateTime(2024, 9, 9, 4, 34, 3, 2, DateTimeKind.Utc).AddTicks(6637),
                            Email = "Raheel@hotmail.com",
                            EmailConfirmed = false,
                            FirstName = "Raheel",
                            LastName = "Sheikh",
                            LockoutEnabled = false,
                            NormalizedEmail = "RAHEEL@HOTMAIL.COM",
                            NormalizedUserName = "RAHEEL",
                            PasswordHash = "AQAAAAIAAYagAAAAEB212wSiD6y38tDcZS0LnTrg/7amyKn6ls13m3rDMyIVzIVhp18WLFbwFOPkYDlCAg==",
                            PhoneNumberConfirmed = false,
                            RefreshToken = "",
                            RefreshTokenExpiryTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SecurityStamp = "9723be5c-1a46-427f-8310-7d879fe97c63",
                            TwoFactorEnabled = false,
                            UserName = "Raheel"
                        },
                        new
                        {
                            Id = "3857779e-aa32-4e1f-9e02-f6a758bc4616",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "dea82089-5e2f-4858-9a3e-b486ffdb852b",
                            DateCreated = new DateTime(2024, 9, 9, 4, 34, 3, 65, DateTimeKind.Utc).AddTicks(6959),
                            DateModified = new DateTime(2024, 9, 9, 4, 34, 3, 65, DateTimeKind.Utc).AddTicks(6963),
                            Email = "Shehneela@hotmail.com",
                            EmailConfirmed = false,
                            FirstName = "Shehneela",
                            LastName = "Khan",
                            LockoutEnabled = false,
                            NormalizedEmail = "SHEHNEELA@HOTMAIL.COM",
                            NormalizedUserName = "SHEHNEELA",
                            PasswordHash = "AQAAAAIAAYagAAAAEC9iQ4KTHFvNzo5a1VN0pDFR1N30MkaF5Ib9MIjaYD1J0ARyR4OxtxMnUz/x93dfhw==",
                            PhoneNumberConfirmed = false,
                            RefreshToken = "",
                            RefreshTokenExpiryTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SecurityStamp = "c840555a-8c22-412d-958c-558604c4f2de",
                            TwoFactorEnabled = false,
                            UserName = "Shehneela Khan"
                        },
                        new
                        {
                            Id = "4e68632f-2771-4305-8001-165fdfe18022",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "4e4dbead-2bf4-40c5-9856-4003b74541bd",
                            DateCreated = new DateTime(2024, 9, 9, 4, 34, 3, 128, DateTimeKind.Utc).AddTicks(188),
                            DateModified = new DateTime(2024, 9, 9, 4, 34, 3, 128, DateTimeKind.Utc).AddTicks(197),
                            Email = "Salman@hotmail.com",
                            EmailConfirmed = false,
                            FirstName = "Salman",
                            LastName = "Khan",
                            LockoutEnabled = false,
                            NormalizedEmail = "SALMAN@HOTMAIL.COM",
                            NormalizedUserName = "SALMAN",
                            PasswordHash = "AQAAAAIAAYagAAAAEOaGVwGQ6EZbkg+yW+WAk+NFA7wNIRBF3KJy8zKeMaPnd0vrJ7lQYmRYHGtNR4EBTA==",
                            PhoneNumberConfirmed = false,
                            RefreshToken = "",
                            RefreshTokenExpiryTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SecurityStamp = "1ffe0c97-7b8f-4f0d-8fec-a46ddb5c2172",
                            TwoFactorEnabled = false,
                            UserName = "Salman Khan"
                        });
                });

            modelBuilder.Entity("GeneralEntries.Models.ChartsofAccounts", b =>
                {
                    b.Property<int>("LedgerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LedgerId"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("CostCenter")
                        .HasColumnType("int");

                    b.Property<string>("HeadofAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LedgerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NatureofAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("OpeningBalance")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("LedgerId");

                    b.HasIndex("CompanyId");

                    b.ToTable("ChartsOfAccounts");
                });

            modelBuilder.Entity("GeneralEntries.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("Branch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("CompanyId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("GeneralEntries.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EmployeeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("EmployeeId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            EmployeeId = 1,
                            Age = 29,
                            ApplicationUserId = "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5",
                            EmployeeName = "Raheel",
                            Salary = 100000m
                        });
                });

            modelBuilder.Entity("GeneralEntries.Models.GeneralEntry", b =>
                {
                    b.Property<int>("EntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EntryId"));

                    b.Property<int>("ChartId")
                        .HasColumnType("int");

                    b.Property<string>("CostCenter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Credit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Debit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("GeneralVoucherId")
                        .HasColumnType("int");

                    b.Property<string>("Narration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelateCompany")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelateLedger")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntryId");

                    b.HasIndex("ChartId");

                    b.HasIndex("GeneralVoucherId");

                    b.ToTable("GeneralEntries");
                });

            modelBuilder.Entity("GeneralEntries.Models.GeneralVoucher", b =>
                {
                    b.Property<int>("GeneralVoucherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GeneralVoucherId"));

                    b.Property<DateTime>("Vdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Voucher")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GeneralVoucherId");

                    b.ToTable("GeneralVouchers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "10bdfb23-a1b3-47be-b614-002ffc63a538",
                            ConcurrencyStamp = "1c90a1ab-7b2e-4d08-ba35-8679c0cb764c",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "5b8c9ca2-f2e2-43bf-ab1c-ab63aa01dd3a",
                            ConcurrencyStamp = "f4f6eba8-8b6b-41b1-98dd-f5f5f3f90ad0",
                            Name = "Manager",
                            NormalizedName = "MANAGER"
                        },
                        new
                        {
                            Id = "f7b1c274-7e98-42b2-a82a-cdb521f9aaee",
                            ConcurrencyStamp = "6d853589-a7e4-46a0-98d3-4af3bf0a6ee5",
                            Name = "Employee",
                            NormalizedName = "EMPLOYEE"
                        },
                        new
                        {
                            Id = "a3f5df59-a0c6-4586-a9f0-431843d3749c",
                            ConcurrencyStamp = "fc476ba7-ecca-4dd0-bf52-2da09fe41532",
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "Admin",
                            ClaimValue = "Add Manager",
                            UserId = "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5"
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "Admin",
                            ClaimValue = "Edit Manager",
                            UserId = "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5"
                        },
                        new
                        {
                            Id = 3,
                            ClaimType = "Admin",
                            ClaimValue = "Delete Manager",
                            UserId = "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5"
                        },
                        new
                        {
                            Id = 4,
                            ClaimType = "Admin",
                            ClaimValue = "Get Manager",
                            UserId = "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5"
                        },
                        new
                        {
                            Id = 5,
                            ClaimType = "Manager",
                            ClaimValue = "Add Employee",
                            UserId = "77b5e980-9439-4d81-922a-b368d1bf3ecf"
                        },
                        new
                        {
                            Id = 6,
                            ClaimType = "Manager",
                            ClaimValue = "Edit Employee",
                            UserId = "77b5e980-9439-4d81-922a-b368d1bf3ecf"
                        },
                        new
                        {
                            Id = 7,
                            ClaimType = "Manager",
                            ClaimValue = "Delete Employee",
                            UserId = "77b5e980-9439-4d81-922a-b368d1bf3ecf"
                        },
                        new
                        {
                            Id = 8,
                            ClaimType = "Manager",
                            ClaimValue = "Get Employee",
                            UserId = "77b5e980-9439-4d81-922a-b368d1bf3ecf"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5",
                            RoleId = "10bdfb23-a1b3-47be-b614-002ffc63a538"
                        },
                        new
                        {
                            UserId = "77b5e980-9439-4d81-922a-b368d1bf3ecf",
                            RoleId = "5b8c9ca2-f2e2-43bf-ab1c-ab63aa01dd3a"
                        },
                        new
                        {
                            UserId = "3857779e-aa32-4e1f-9e02-f6a758bc4616",
                            RoleId = "f7b1c274-7e98-42b2-a82a-cdb521f9aaee"
                        },
                        new
                        {
                            UserId = "4e68632f-2771-4305-8001-165fdfe18022",
                            RoleId = "a3f5df59-a0c6-4586-a9f0-431843d3749c"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("GeneralEntries.Models.ChartsofAccounts", b =>
                {
                    b.HasOne("GeneralEntries.Models.Company", "Company")
                        .WithMany("Charts")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("GeneralEntries.Models.Company", b =>
                {
                    b.HasOne("GeneralEntries.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("GeneralEntries.Models.Employee", b =>
                {
                    b.HasOne("GeneralEntries.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Employees")
                        .HasForeignKey("ApplicationUserId");

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("GeneralEntries.Models.GeneralEntry", b =>
                {
                    b.HasOne("GeneralEntries.Models.ChartsofAccounts", "Chart")
                        .WithMany("AllEntries")
                        .HasForeignKey("ChartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GeneralEntries.Models.GeneralVoucher", "GeneralVoucher")
                        .WithMany("GenEntries")
                        .HasForeignKey("GeneralVoucherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chart");

                    b.Navigation("GeneralVoucher");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GeneralEntries.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GeneralEntries.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GeneralEntries.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("GeneralEntries.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GeneralEntries.Models.ApplicationUser", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("GeneralEntries.Models.ChartsofAccounts", b =>
                {
                    b.Navigation("AllEntries");
                });

            modelBuilder.Entity("GeneralEntries.Models.Company", b =>
                {
                    b.Navigation("Charts");
                });

            modelBuilder.Entity("GeneralEntries.Models.GeneralVoucher", b =>
                {
                    b.Navigation("GenEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
