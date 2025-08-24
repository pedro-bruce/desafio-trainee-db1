using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddExportFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExportedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "WasExported",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExportedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WasExported",
                table: "Products");
        }
    }
}
