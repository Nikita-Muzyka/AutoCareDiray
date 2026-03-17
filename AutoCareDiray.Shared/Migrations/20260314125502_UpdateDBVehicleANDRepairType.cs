using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCareDiray.Shared.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBVehicleANDRepairType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemoveMaintenance",
                table: "RepairTypes",
                newName: "IsRemoveMaintenance");

            migrationBuilder.AddColumn<string>(
                name: "TransmissionType",
                table: "Vehicles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransmissionType",
                table: "RepairTypes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransmissionType",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TransmissionType",
                table: "RepairTypes");

            migrationBuilder.RenameColumn(
                name: "IsRemoveMaintenance",
                table: "RepairTypes",
                newName: "RemoveMaintenance");
        }
    }
}
