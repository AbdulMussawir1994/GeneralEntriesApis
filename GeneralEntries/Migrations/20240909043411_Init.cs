using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GeneralEntries.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralVouchers",
                columns: table => new
                {
                    GeneralVoucherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Voucher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralVouchers", x => x.GeneralVoucherId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_Companies_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChartsOfAccounts",
                columns: table => new
                {
                    LedgerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LedgerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadofAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureofAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostCenter = table.Column<int>(type: "int", nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartsOfAccounts", x => x.LedgerId);
                    table.ForeignKey(
                        name: "FK_ChartsOfAccounts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralEntries",
                columns: table => new
                {
                    EntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralVoucherId = table.Column<int>(type: "int", nullable: false),
                    VNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelateCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelateLedger = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralEntries", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_GeneralEntries_ChartsOfAccounts_ChartId",
                        column: x => x.ChartId,
                        principalTable: "ChartsOfAccounts",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralEntries_GeneralVouchers_GeneralVoucherId",
                        column: x => x.GeneralVoucherId,
                        principalTable: "GeneralVouchers",
                        principalColumn: "GeneralVoucherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10bdfb23-a1b3-47be-b614-002ffc63a538", "1c90a1ab-7b2e-4d08-ba35-8679c0cb764c", "Admin", "ADMIN" },
                    { "5b8c9ca2-f2e2-43bf-ab1c-ab63aa01dd3a", "f4f6eba8-8b6b-41b1-98dd-f5f5f3f90ad0", "Manager", "MANAGER" },
                    { "a3f5df59-a0c6-4586-a9f0-431843d3749c", "fc476ba7-ecca-4dd0-bf52-2da09fe41532", "User", "USER" },
                    { "f7b1c274-7e98-42b2-a82a-cdb521f9aaee", "6d853589-a7e4-46a0-98d3-4af3bf0a6ee5", "Employee", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateCreated", "DateModified", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "3857779e-aa32-4e1f-9e02-f6a758bc4616", 0, "dea82089-5e2f-4858-9a3e-b486ffdb852b", new DateTime(2024, 9, 9, 4, 34, 3, 65, DateTimeKind.Utc).AddTicks(6959), new DateTime(2024, 9, 9, 4, 34, 3, 65, DateTimeKind.Utc).AddTicks(6963), "Shehneela@hotmail.com", false, "Shehneela", "Khan", false, null, "SHEHNEELA@HOTMAIL.COM", "SHEHNEELA", "AQAAAAIAAYagAAAAEC9iQ4KTHFvNzo5a1VN0pDFR1N30MkaF5Ib9MIjaYD1J0ARyR4OxtxMnUz/x93dfhw==", null, false, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "c840555a-8c22-412d-958c-558604c4f2de", false, "Shehneela Khan" },
                    { "4e68632f-2771-4305-8001-165fdfe18022", 0, "4e4dbead-2bf4-40c5-9856-4003b74541bd", new DateTime(2024, 9, 9, 4, 34, 3, 128, DateTimeKind.Utc).AddTicks(188), new DateTime(2024, 9, 9, 4, 34, 3, 128, DateTimeKind.Utc).AddTicks(197), "Salman@hotmail.com", false, "Salman", "Khan", false, null, "SALMAN@HOTMAIL.COM", "SALMAN", "AQAAAAIAAYagAAAAEOaGVwGQ6EZbkg+yW+WAk+NFA7wNIRBF3KJy8zKeMaPnd0vrJ7lQYmRYHGtNR4EBTA==", null, false, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1ffe0c97-7b8f-4f0d-8fec-a46ddb5c2172", false, "Salman Khan" },
                    { "77b5e980-9439-4d81-922a-b368d1bf3ecf", 0, "4502a6cf-7f54-4c26-9d68-158471c51909", new DateTime(2024, 9, 9, 4, 34, 3, 2, DateTimeKind.Utc).AddTicks(6633), new DateTime(2024, 9, 9, 4, 34, 3, 2, DateTimeKind.Utc).AddTicks(6637), "Raheel@hotmail.com", false, "Raheel", "Sheikh", false, null, "RAHEEL@HOTMAIL.COM", "RAHEEL", "AQAAAAIAAYagAAAAEB212wSiD6y38tDcZS0LnTrg/7amyKn6ls13m3rDMyIVzIVhp18WLFbwFOPkYDlCAg==", null, false, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "9723be5c-1a46-427f-8310-7d879fe97c63", false, "Raheel" },
                    { "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5", 0, "fac60314-3509-4de3-a9dc-6fa1d6526016", new DateTime(2024, 9, 9, 4, 34, 2, 940, DateTimeKind.Utc).AddTicks(7408), new DateTime(2024, 9, 9, 4, 34, 2, 940, DateTimeKind.Utc).AddTicks(7409), "Abdul_mussawir@hotmail.com", false, "AbdulMussawir", "Sheikh", false, null, "ABDUL_MUSSAWIR@HOTMAIL.COM", "ABDULMUSSAWIR", "AQAAAAIAAYagAAAAEMYTaZIyZEz9Ho7BniOx7sxcM6kEP7H+t8IqoF97q7d75I8xrbkgvIULZUQI61reXg==", null, false, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "5f1d0701-26f5-4e49-a421-798fc340e966", false, "Abdul Mussawir" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "Admin", "Add Manager", "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5" },
                    { 2, "Admin", "Edit Manager", "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5" },
                    { 3, "Admin", "Delete Manager", "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5" },
                    { 4, "Admin", "Get Manager", "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5" },
                    { 5, "Manager", "Add Employee", "77b5e980-9439-4d81-922a-b368d1bf3ecf" },
                    { 6, "Manager", "Edit Employee", "77b5e980-9439-4d81-922a-b368d1bf3ecf" },
                    { 7, "Manager", "Delete Employee", "77b5e980-9439-4d81-922a-b368d1bf3ecf" },
                    { 8, "Manager", "Get Employee", "77b5e980-9439-4d81-922a-b368d1bf3ecf" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "f7b1c274-7e98-42b2-a82a-cdb521f9aaee", "3857779e-aa32-4e1f-9e02-f6a758bc4616" },
                    { "a3f5df59-a0c6-4586-a9f0-431843d3749c", "4e68632f-2771-4305-8001-165fdfe18022" },
                    { "5b8c9ca2-f2e2-43bf-ab1c-ab63aa01dd3a", "77b5e980-9439-4d81-922a-b368d1bf3ecf" },
                    { "10bdfb23-a1b3-47be-b614-002ffc63a538", "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "ApplicationUserId", "EmployeeName", "Salary" },
                values: new object[] { 1, 29, "a27bcbae-b26c-4ce9-ab22-fc9c1f85d2d5", "Raheel", 100000m });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChartsOfAccounts_CompanyId",
                table: "ChartsOfAccounts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_EmployeeId",
                table: "Companies",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralEntries_ChartId",
                table: "GeneralEntries",
                column: "ChartId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralEntries_GeneralVoucherId",
                table: "GeneralEntries",
                column: "GeneralVoucherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "GeneralEntries");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ChartsOfAccounts");

            migrationBuilder.DropTable(
                name: "GeneralVouchers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
