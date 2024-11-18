using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PurchaseSiteParser.Migrations
{
    /// <inheritdoc />
    public partial class PurchaseCardsAndResultsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseParsingResults",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseName = table.Column<string>(type: "text", nullable: false),
                    PagesPeriod = table.Column<string>(type: "text", nullable: false),
                    PurchasesListCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseParsingResults", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Law = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    PurchaseObject = table.Column<string>(type: "text", nullable: false),
                    Organization = table.Column<string>(type: "text", nullable: false),
                    StartPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    PurchaseParsingResultId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseCards_PurchaseParsingResults_PurchaseParsingResultId",
                        column: x => x.PurchaseParsingResultId,
                        principalTable: "PurchaseParsingResults",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseCards_PurchaseParsingResultId",
                table: "PurchaseCards",
                column: "PurchaseParsingResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseCards");

            migrationBuilder.DropTable(
                name: "PurchaseParsingResults");
        }
    }
}
