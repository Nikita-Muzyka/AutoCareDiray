using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCareDiray.Shared.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRepairType2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntervalDate",
                table: "RepairTypes");

            migrationBuilder.AddColumn<int>(
                name: "IntervalMonth",
                table: "RepairTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntervalMonth",
                table: "RepairTypes");

            migrationBuilder.AddColumn<DateTime>(
                name: "IntervalDate",
                table: "RepairTypes",
                type: "TEXT",
                nullable: true);
        }
    }
}
