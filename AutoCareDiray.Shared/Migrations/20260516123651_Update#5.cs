using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCareDiray.Shared.Migrations
{
    /// <inheritdoc />
    public partial class Update5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FuelTank",
                table: "Vehicles",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VolumeLiters",
                table: "Vehicles",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Repairs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "Refills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateRefill = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Mileage = table.Column<int>(type: "INTEGER", nullable: false),
                    VolumeLiters = table.Column<double>(type: "REAL", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false),
                    FuelType = table.Column<string>(type: "TEXT", nullable: false),
                    IsFullTank = table.Column<bool>(type: "INTEGER", nullable: false),
                    GasStationName = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Photos = table.Column<string>(type: "TEXT", nullable: true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refills_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Refills_VehicleId",
                table: "Refills",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Refills");

            migrationBuilder.DropColumn(
                name: "FuelTank",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VolumeLiters",
                table: "Vehicles");

            migrationBuilder.AlterColumn<int>(
                name: "Cost",
                table: "Repairs",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }
    }
}
