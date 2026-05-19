using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCareDiray.Shared.Migrations
{
    /// <inheritdoc />
    public partial class Update7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpareParts",
                table: "Repairs");

            migrationBuilder.CreateTable(
                name: "SparePart",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NamePart = table.Column<string>(type: "TEXT", nullable: false),
                    ArticleNumberPart = table.Column<string>(type: "TEXT", nullable: false),
                    CostPart = table.Column<decimal>(type: "TEXT", nullable: false),
                    RepairId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePart", x => x.id);
                    table.ForeignKey(
                        name: "FK_SparePart_Repairs_RepairId",
                        column: x => x.RepairId,
                        principalTable: "Repairs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SparePart_RepairId",
                table: "SparePart",
                column: "RepairId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SparePart");

            migrationBuilder.AddColumn<string>(
                name: "SpareParts",
                table: "Repairs",
                type: "TEXT",
                nullable: true);
        }
    }
}
