using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCareDiray.Shared.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRepairType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "IntervalDate",
                table: "RepairTypes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<bool>(
                name: "IsServiced",
                table: "RepairTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LastServiceMileage",
                table: "RepairTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsServiced",
                table: "RepairTypes");

            migrationBuilder.DropColumn(
                name: "LastServiceMileage",
                table: "RepairTypes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IntervalDate",
                table: "RepairTypes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
