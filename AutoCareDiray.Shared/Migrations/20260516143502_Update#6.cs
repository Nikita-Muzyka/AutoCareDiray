using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCareDiray.Shared.Migrations
{
    /// <inheritdoc />
    public partial class Update6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VehicleNotes_VehicleId",
                table: "VehicleNotes",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleNotes_Vehicles_VehicleId",
                table: "VehicleNotes",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleNotes_Vehicles_VehicleId",
                table: "VehicleNotes");

            migrationBuilder.DropIndex(
                name: "IX_VehicleNotes_VehicleId",
                table: "VehicleNotes");
        }
    }
}
