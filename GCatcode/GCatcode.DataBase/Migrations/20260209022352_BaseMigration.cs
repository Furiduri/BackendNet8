using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GCatcode.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class BaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CL_ControllerMethods",
                columns: table => new
                {
                    MethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControllerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HttpMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PermissionType = table.Column<int>(type: "int", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CL_ControllerMethods", x => x.MethodId);
                });

            migrationBuilder.CreateTable(
                name: "CL_Controllers",
                columns: table => new
                {
                    ControllerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ControllerPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Available = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CL_Controllers", x => x.ControllerId);
                });

            migrationBuilder.CreateTable(
                name: "CL_Roles",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Available = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CL_Roles", x => x.RolId);
                });

            migrationBuilder.CreateTable(
                name: "CL_Views",
                columns: table => new
                {
                    ViewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Route = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentViewId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CL_Views", x => x.ViewId);
                });

            migrationBuilder.CreateTable(
                name: "TR_Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TR_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RL_ViewControllers",
                columns: table => new
                {
                    ViewId = table.Column<int>(type: "int", nullable: false),
                    ControllerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RL_ViewControllers", x => new { x.ViewId, x.ControllerId });
                    table.ForeignKey(
                        name: "FK_RL_ViewControllers_CL_Controllers_ControllerId",
                        column: x => x.ControllerId,
                        principalTable: "CL_Controllers",
                        principalColumn: "ControllerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RL_ViewControllers_CL_Views_ViewId",
                        column: x => x.ViewId,
                        principalTable: "CL_Views",
                        principalColumn: "ViewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RL_ViewRoles",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "int", nullable: false),
                    ViewId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RL_ViewRoles", x => new { x.ViewId, x.RolId });
                    table.ForeignKey(
                        name: "FK_RL_ViewRoles_CL_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "CL_Roles",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RL_ViewRoles_CL_Views_ViewId",
                        column: x => x.ViewId,
                        principalTable: "CL_Views",
                        principalColumn: "ViewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RL_UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RL_UserRoles", x => new { x.UserId, x.RolId });
                    table.ForeignKey(
                        name: "FK_RL_UserRoles_CL_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "CL_Roles",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RL_UserRoles_TR_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "TR_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TR_RefreshTokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TR_RefreshTokens", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users",
                        column: x => x.UserId,
                        principalTable: "TR_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CL_ControllerMethods",
                columns: new[] { "MethodId", "Available", "ControllerId", "CreateTime", "Description", "Endpoint", "HttpMethod", "LastUpdated", "Name", "PermissionType" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Listar usuarios", "/admin/Users", "GET", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GetAll", 1 },
                    { 2, true, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Obtener usuario por ID", "/admin/Users/{id}", "GET", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GetById", 1 },
                    { 3, true, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crear usuario", "/admin/Users", "POST", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Create", 2 },
                    { 4, true, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Actualizar usuario", "/admin/Users/{id}", "PUT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Update", 2 },
                    { 5, true, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Eliminar usuario", "/admin/Users/{id}", "DELETE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Delete", 3 },
                    { 6, true, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Listar roles", "/admin/Roles", "GET", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GetAll", 1 },
                    { 7, true, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Obtener rol por ID", "/admin/Roles/{id}", "GET", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GetById", 1 },
                    { 8, true, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crear rol", "/admin/Roles", "POST", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Create", 2 },
                    { 9, true, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Actualizar rol", "/admin/Roles/{id}", "PUT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Update", 2 },
                    { 10, true, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Eliminar rol", "/admin/Roles/{id}", "DELETE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Delete", 3 }
                });

            migrationBuilder.InsertData(
                table: "CL_Controllers",
                columns: new[] { "ControllerId", "Available", "ControllerPath", "CreateTime", "Description", "LastUpdated", "Name" },
                values: new object[,]
                {
                    { 1, true, "admin/users", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Controller for users management", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "UsersController" },
                    { 2, true, "admin/roles", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Controller for roles management", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "RolesController" },
                    { 3, true, "admin/permissions", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Controller for permissions management", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PermissionsController" }
                });

            migrationBuilder.InsertData(
                table: "CL_Roles",
                columns: new[] { "RolId", "Available", "CreateTime", "Description", "LastUpdated", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "All Access Development", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dev" },
                    { 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Limited Access", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Guest" },
                    { 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Access parcial", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "User" },
                    { 4, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "All Access Administration", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "CL_Views",
                columns: new[] { "ViewId", "Available", "CreateTime", "Description", "Icon", "IsActive", "LastUpdated", "Name", "Order", "ParentViewId", "Route" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "DashboardIcon", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dashboard", 1, null, "/dashboard" },
                    { 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View for Adim tools", "admin", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin", 0, null, "admin" },
                    { 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View for users management", "user", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Users", 0, 1, "admin/users" },
                    { 4, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View for roles management", "list", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Roles", 0, 1, "admin/roles" },
                    { 5, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View for permisos management", "auth", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permisos", 0, 1, "admin/permisos" }
                });

            migrationBuilder.InsertData(
                table: "TR_Users",
                columns: new[] { "UserId", "Available", "CreateTime", "Email", "LastUpdated", "Password", "UserName" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "dev@local.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bDN+lqTwOni3VvDVCpMZ/fdIOeLJWI4xr/pBMFxpmXrM2eKXisTjyE3Jl22qlVez", "Dev" },
                    { 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "guest@local.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ovs8iCBxayixik0xCdcm9juL4VTn7mJeScrfiWEyZdXRZi9N7pWb5qZoY4Ry+dpL", "Guest" },
                    { 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "user@local.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "LVrRLFuwP0EOJ9QAkhoWJFx8tab4KxUrGo1ME8wZRWHvM2saRRM3yAl1wLdBkPsu", "User" },
                    { 4, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@local.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pUoDuxUl0hWFYdvT+8Jg7TbBM6e634yhjrLE19bOTdODOpLoU8a3RRJIvhs2zAmf", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "RL_UserRoles",
                columns: new[] { "RolId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "RL_ViewControllers",
                columns: new[] { "ControllerId", "ViewId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 2, 4 },
                    { 3, 5 }
                });

            migrationBuilder.InsertData(
                table: "RL_ViewRoles",
                columns: new[] { "RolId", "ViewId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 1, 3 },
                    { 4, 3 },
                    { 1, 4 },
                    { 4, 4 },
                    { 1, 5 },
                    { 4, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CL_ControllerMethods_ControllerId_Name",
                table: "CL_ControllerMethods",
                columns: new[] { "ControllerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RL_UserRoles_RolId",
                table: "RL_UserRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_RL_ViewControllers_ControllerId",
                table: "RL_ViewControllers",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_RL_ViewRoles_RolId",
                table: "RL_ViewRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ExpiryDate_IsRevoked",
                table: "TR_RefreshTokens",
                columns: new[] { "ExpiryDate", "IsRevoked" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "TR_RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "TR_RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TR_Users_Email",
                table: "TR_Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TR_Users_UserName",
                table: "TR_Users",
                column: "UserName",
                unique: true);

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[sp_CleanExpiredRefreshTokens]
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @DeletedCount INT;

                    DELETE FROM [dbo].[TR_RefreshTokens]
                    WHERE ExpiryDate < GETUTCDATE();

                    SET @DeletedCount = @@ROWCOUNT;

                    PRINT CONCAT('Tokens expirados eliminados: ', @DeletedCount);
                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[sp_CleanExpiredRefreshTokens]");
            migrationBuilder.DropTable(
                name: "CL_ControllerMethods");

            migrationBuilder.DropTable(
                name: "RL_UserRoles");

            migrationBuilder.DropTable(
                name: "RL_ViewControllers");

            migrationBuilder.DropTable(
                name: "RL_ViewRoles");

            migrationBuilder.DropTable(
                name: "TR_RefreshTokens");

            migrationBuilder.DropTable(
                name: "CL_Controllers");

            migrationBuilder.DropTable(
                name: "CL_Roles");

            migrationBuilder.DropTable(
                name: "CL_Views");

            migrationBuilder.DropTable(
                name: "TR_Users");
        }
    }
}