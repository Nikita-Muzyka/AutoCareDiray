using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCareDiray.Shared.Migrations
{
    /// <inheritdoc />
    public partial class Update8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SparePart_Repairs_RepairId",
                table: "SparePart");

            migrationBuilder.AddForeignKey(
                name: "FK_SparePart_Repairs_RepairId",
                table: "SparePart",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SparePart_Repairs_RepairId",
                table: "SparePart");

            migrationBuilder.AddForeignKey(
                name: "FK_SparePart_Repairs_RepairId",
                table: "SparePart",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "Id");
        }
    }
}
