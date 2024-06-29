using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToTaskWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("01547ccb-69cc-4042-9927-a5d895b007c9"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2576eb02-fa3e-4425-b559-5d1d3850a320"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7a9905cc-9a4f-4a3d-b179-6400c8c105d5"));

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Tasks");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("01547ccb-69cc-4042-9927-a5d895b007c9"), "Manager" },
                    { new Guid("2576eb02-fa3e-4425-b559-5d1d3850a320"), "Admin" },
                    { new Guid("7a9905cc-9a4f-4a3d-b179-6400c8c105d5"), "User" }
                });
        }
    }
}
