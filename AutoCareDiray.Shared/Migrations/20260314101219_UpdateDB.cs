using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCareDiray.Shared.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VinCode",
                table: "Vehicles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "RepairTypes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RemoveMaintenance",
                table: "RepairTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VinCode",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "RepairTypes");

            migrationBuilder.DropColumn(
                name: "RemoveMaintenance",
                table: "RepairTypes");
        }
    }
}
