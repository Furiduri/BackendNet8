using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GCatcode.SQLServerDatabase.Migrations
{
    /// <inheritdoc />
    public partial class UserUserRolTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Available = table.Column<bool>(type: "bit", defaultValue: true, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", defaultValue: DateTime.UtcNow, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", defaultValue: DateTime.UtcNow, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    Available = table.Column<bool>(type: "bit", defaultValue: true, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", defaultValue: DateTime.UtcNow, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", defaultValue: DateTime.UtcNow, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RolId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RolId",
                table: "UserRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "UserName", "Email" , "Password" },
                values: new object[,]
                {
                    {0, "Dev", null , "12345" },
                    {1, "Guest", null ,"123345" },
                    {2, "User", null ,"12345" },
                    {3, "Admin", null , "12345"}
            });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserId", "RolId" },
                values: new object[,]
                {
                    {0, 0 },
                    {0, 1 },
                    {0, 2 },
                    {0, 3 },
                    {1, 1 },
                    {2, 2 },
                    {3, 3}
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
