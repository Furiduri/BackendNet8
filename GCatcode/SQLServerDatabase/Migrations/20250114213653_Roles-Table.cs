using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GCatcode.SQLServerDatabase.Migrations
{
    /// <inheritdoc />
    public partial class RolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Available = table.Column<bool>(type: "bit", defaultValue: true, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", defaultValue: DateTime.UtcNow, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", defaultValue: DateTime.UtcNow, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolId);
                }
            );

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RolId", "Name", "Description" },
                values: new object[,]
                {
                    {0, "Dev", "All Access Development" },
                    {1, "Guest", "Limited Access" },
                    {2, "User", "Access parcial" },
                    {3, "Admin" , "All Access Administration"}
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}