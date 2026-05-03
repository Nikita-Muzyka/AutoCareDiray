using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCareDiray.Shared.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehicle2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfFile",
                table: "Vehicles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfFile",
                table: "Vehicles");
        }
    }
}
