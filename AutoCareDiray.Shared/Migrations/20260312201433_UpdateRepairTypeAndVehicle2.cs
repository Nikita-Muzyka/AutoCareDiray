using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCareDiray.Shared.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRepairTypeAndVehicle2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IntervalMileagee",
                table: "RepairTypes",
                newName: "IntervalMileage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IntervalMileage",
                table: "RepairTypes",
                newName: "IntervalMileagee");
        }
    }
}
