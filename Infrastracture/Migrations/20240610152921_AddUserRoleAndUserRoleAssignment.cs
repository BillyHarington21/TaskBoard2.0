using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleAndUserRoleAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("27ba8532-bf04-490c-a984-4a2c6d4b0ba0"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("28083139-7a50-41c1-a058-e6d41f441924"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("63bfe298-acba-4acf-8043-a685cdd53b8f"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("7c6cbc91-c2b8-4e91-9ebb-8a968066fc38"), "Admin" },
                    { new Guid("7f141939-8d7a-452a-80b6-c68312ed1e8d"), "Manager" },
                    { new Guid("832cbaa7-1b96-4804-a640-6a7fa90553c2"), "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7c6cbc91-c2b8-4e91-9ebb-8a968066fc38"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7f141939-8d7a-452a-80b6-c68312ed1e8d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("832cbaa7-1b96-4804-a640-6a7fa90553c2"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("27ba8532-bf04-490c-a984-4a2c6d4b0ba0"), "User" },
                    { new Guid("28083139-7a50-41c1-a058-e6d41f441924"), "Manager" },
                    { new Guid("63bfe298-acba-4acf-8043-a685cdd53b8f"), "Admin" }
                });
        }
    }
}
