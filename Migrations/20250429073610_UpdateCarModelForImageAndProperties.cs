using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SashoApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarModelForImageAndProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "Mileage",
                table: "Cars",
                newName: "NumberOfDoors");

            migrationBuilder.AddColumn<string>(
                name: "EngineType",
                table: "Cars",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "HorsePower",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Cars",
                type: "longblob",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngineType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "HorsePower",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "NumberOfDoors",
                table: "Cars",
                newName: "Mileage");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cars",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Cars",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
