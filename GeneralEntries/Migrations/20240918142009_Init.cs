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
                    CompanyName = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
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
                    { "12907185-2c80-4e90-b603-384a2c0a19c0", "e91dd84e-01a2-4902-86f5-724a185587b4", "Admin", "ADMIN" },
                    { "85611d21-5abd-4aa3-88ef-fbbf55b16b94", "42d39c11-eb0c-4449-9d22-d1588b50b71d", "Manager", "MANAGER" },
                    { "bc7efa71-84dc-45ef-a38b-fa42aba10733", "885a06e4-d716-4aef-850e-2b86a42954c4", "User", "USER" },
                    { "cccfbda6-4b61-4781-93bf-e97a2d26a85b", "8e845b82-9000-4a82-8ad8-fb1c8b9fa760", "Employee", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateCreated", "DateModified", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0d887aff-dc9f-4903-bfe6-86c468d7db6d", 0, "ef6458c6-f23a-4303-9309-404d47295c4e", new DateTime(2024, 9, 18, 14, 20, 6, 200, DateTimeKind.Utc).AddTicks(4998), new DateTime(2024, 9, 18, 14, 20, 6, 200, DateTimeKind.Utc).AddTicks(5003), "Salman@hotmail.com", false, "Salman", "Khan", false, null, "SALMAN@HOTMAIL.COM", "SALMAN", "AQAAAAIAAYagAAAAEDUOmapXr4bOgnuWflx4bfBiWZwHg7MhNRmCOQ243ajRSF4L7NCsJughm5VajBOU6A==", null, false, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0fbee2d2-11fa-4d90-93fb-62b60517b2cc", false, "Salman Khan" },
                    { "37a286c2-e2f2-4ccc-9f9c-f8592b6af924", 0, "0bb30423-3db6-4d2a-80d0-ac8a43575d8a", new DateTime(2024, 9, 18, 14, 20, 6, 126, DateTimeKind.Utc).AddTicks(2171), new DateTime(2024, 9, 18, 14, 20, 6, 126, DateTimeKind.Utc).AddTicks(2176), "Shehneela@hotmail.com", false, "Shehneela", "Khan", false, null, "SHEHNEELA@HOTMAIL.COM", "SHEHNEELA", "AQAAAAIAAYagAAAAEF51ioQQW8kkPKKbMnjwHL01zxNKnlTtJnrFZ55V7G97YSxYNEJ+oBP8iiSxHY1z2Q==", null, false, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2f9a16c5-ae90-45be-861a-140f569de5be", false, "Shehneela Khan" },
                    { "4e977b81-8b10-4bea-b18c-75b4495e7439", 0, "4097dab3-5ac6-41fb-b64a-81ff0bc3e2c6", new DateTime(2024, 9, 18, 14, 20, 5, 998, DateTimeKind.Utc).AddTicks(7692), new DateTime(2024, 9, 18, 14, 20, 5, 998, DateTimeKind.Utc).AddTicks(7695), "Abdul_mussawir@hotmail.com", false, "AbdulMussawir", "Sheikh", false, null, "ABDUL_MUSSAWIR@HOTMAIL.COM", "ABDULMUSSAWIR", "AQAAAAIAAYagAAAAEEyKE23rjw+WWAZevpmhOUxDYI10+im0REU1VsMEtbCyNW64YU0/mft8ECIuHRVuCA==", null, false, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "039352da-a42f-4bc0-8a25-7e540a473eb3", false, "Abdul Mussawir" },
                    { "a3f6a564-11ed-4727-b878-9932ce5d0f4e", 0, "4a049524-802a-4314-ba44-ea406842a689", new DateTime(2024, 9, 18, 14, 20, 6, 63, DateTimeKind.Utc).AddTicks(5738), new DateTime(2024, 9, 18, 14, 20, 6, 63, DateTimeKind.Utc).AddTicks(5744), "Raheel@hotmail.com", false, "Raheel", "Sheikh", false, null, "RAHEEL@HOTMAIL.COM", "RAHEEL", "AQAAAAIAAYagAAAAEOZ6nSx+SYfPrITRE5bBS3KGS5akB41FmLTltst+rlTtqT+Ejo6u9FZ6JyuX7/KM8w==", null, false, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3f6a1751-3863-4233-bf19-d5e0ca5e6e86", false, "Raheel" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "Admin", "Add Manager", "4e977b81-8b10-4bea-b18c-75b4495e7439" },
                    { 2, "Admin", "Edit Manager", "4e977b81-8b10-4bea-b18c-75b4495e7439" },
                    { 3, "Admin", "Delete Manager", "4e977b81-8b10-4bea-b18c-75b4495e7439" },
                    { 4, "Admin", "Get Manager", "4e977b81-8b10-4bea-b18c-75b4495e7439" },
                    { 5, "Manager", "Add Employee", "a3f6a564-11ed-4727-b878-9932ce5d0f4e" },
                    { 6, "Manager", "Edit Employee", "a3f6a564-11ed-4727-b878-9932ce5d0f4e" },
                    { 7, "Manager", "Delete Employee", "a3f6a564-11ed-4727-b878-9932ce5d0f4e" },
                    { 8, "Manager", "Get Employee", "a3f6a564-11ed-4727-b878-9932ce5d0f4e" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "bc7efa71-84dc-45ef-a38b-fa42aba10733", "0d887aff-dc9f-4903-bfe6-86c468d7db6d" },
                    { "cccfbda6-4b61-4781-93bf-e97a2d26a85b", "37a286c2-e2f2-4ccc-9f9c-f8592b6af924" },
                    { "12907185-2c80-4e90-b603-384a2c0a19c0", "4e977b81-8b10-4bea-b18c-75b4495e7439" },
                    { "85611d21-5abd-4aa3-88ef-fbbf55b16b94", "a3f6a564-11ed-4727-b878-9932ce5d0f4e" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "ApplicationUserId", "EmployeeName", "Salary" },
                values: new object[] { 1, 29, "4e977b81-8b10-4bea-b18c-75b4495e7439", "Raheel", 100000m });

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
                name: "IX_Companies_CompanyName",
                table: "Companies",
                column: "CompanyName",
                unique: true,
                filter: "[CompanyName] IS NOT NULL");

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
