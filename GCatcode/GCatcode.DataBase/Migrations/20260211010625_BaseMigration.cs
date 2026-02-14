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
                name: "CL_Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Resource = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Available = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CL_Permissions", x => x.PermissionId);
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
                name: "RL_RolePermissions",
                columns: table => new
                {
                    RolePermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RL_RolePermissions", x => x.RolePermissionId);
                    table.ForeignKey(
                        name: "FK_RL_RolePermissions_CL_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "CL_Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RL_RolePermissions_CL_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "CL_Roles",
                        principalColumn: "RolId",
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
                name: "RL_UserPermissions",
                columns: table => new
                {
                    UserPermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Available = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RL_UserPermissions", x => x.UserPermissionId);
                    table.ForeignKey(
                        name: "FK_RL_UserPermissions_CL_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "CL_Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RL_UserPermissions_TR_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "TR_Users",
                        principalColumn: "UserId",
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
                table: "CL_Permissions",
                columns: new[] { "PermissionId", "Action", "Available", "Code", "CreateTime", "Description", "LastUpdated", "Resource" },
                values: new object[,]
                {
                    { 1, "read", true, "users.read", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ver usuarios", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "users" },
                    { 2, "write", true, "users.write", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crear/Editar usuarios", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "users" },
                    { 3, "delete", true, "users.delete", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Eliminar usuarios", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "users" },
                    { 4, "read", true, "roles.read", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ver roles", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "roles" },
                    { 5, "write", true, "roles.write", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crear/Editar roles", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "roles" },
                    { 6, "delete", true, "roles.delete", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Eliminar roles", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "roles" },
                    { 7, "read", true, "permissions.read", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ver permisos", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "permissions" },
                    { 8, "write", true, "permissions.write", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Asignar permisos", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "permissions" },
                    { 9, "read", true, "views.read", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ver vistas/menús", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "views" },
                    { 10, "write", true, "views.write", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gestionar vistas/menús", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "views" },
                    { 11, "admin", true, "system.admin", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Administración total del sistema", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 12, "config", true, "system.config", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Configurar sistema", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system" }
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
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "DashboardIcon", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dashboard", 1, null, "/dashboard" },
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
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "dev@local.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "B8u/V5sy0wC7ZQSCtBSQqnl82wdSzXwjeVFpjzkNI4QPB8t0RMkHMV71/qRkgKJ0", "Dev" },
                    { 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "guest@local.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "X3ZupnMNsxBnNefH1y+u+GLpvG07hz3cRN7C/NlP7wJDL/QniwW+TkEnqPb2jwtJ", "Guest" },
                    { 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "user@local.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "NQ+/TPuJltbMz2S/Anq+K/OQq6NRUtJ1iTEYN+sDTqQ2GmGHaxnhxaGiOWY197Bc", "User" },
                    { 4, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@local.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ax6nHg2mDM1aYEdX0Sh8wTPw90jQuF2+iDqu2IjyvgbBTpGNzigf4WuO98nTgB6n", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "RL_RolePermissions",
                columns: new[] { "RolePermissionId", "Available", "CreateTime", "LastUpdated", "PermissionId", "RolId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                    { 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 1 },
                    { 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 1 },
                    { 4, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, 1 },
                    { 5, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, 1 },
                    { 6, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, 1 },
                    { 7, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, 1 },
                    { 8, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, 1 },
                    { 9, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, 1 },
                    { 10, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, 1 },
                    { 11, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, 1 },
                    { 12, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, 1 },
                    { 13, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 4 },
                    { 14, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 4 },
                    { 15, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 4 },
                    { 16, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, 4 },
                    { 17, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, 4 },
                    { 18, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, 4 },
                    { 19, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, 4 },
                    { 20, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, 4 },
                    { 21, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, 4 },
                    { 22, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3 },
                    { 23, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, 3 },
                    { 24, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, 3 },
                    { 25, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, 2 }
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
                name: "IX_CL_Permissions_Code",
                table: "CL_Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CL_Permissions_Resource_Action",
                table: "CL_Permissions",
                columns: new[] { "Resource", "Action" });

            migrationBuilder.CreateIndex(
                name: "IX_RL_RolePermissions_PermissionId",
                table: "RL_RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RL_RolePermissions_RolId_PermissionId",
                table: "RL_RolePermissions",
                columns: new[] { "RolId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RL_UserPermissions_PermissionId",
                table: "RL_UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RL_UserPermissions_UserId_PermissionId",
                table: "RL_UserPermissions",
                columns: new[] { "UserId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RL_UserRoles_RolId",
                table: "RL_UserRoles",
                column: "RolId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RL_RolePermissions");

            migrationBuilder.DropTable(
                name: "RL_UserPermissions");

            migrationBuilder.DropTable(
                name: "RL_UserRoles");

            migrationBuilder.DropTable(
                name: "RL_ViewRoles");

            migrationBuilder.DropTable(
                name: "TR_RefreshTokens");

            migrationBuilder.DropTable(
                name: "CL_Permissions");

            migrationBuilder.DropTable(
                name: "CL_Roles");

            migrationBuilder.DropTable(
                name: "CL_Views");

            migrationBuilder.DropTable(
                name: "TR_Users");
        }
    }
}
