using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinkoffInvest.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Securities",
                columns: table => new
                {
                    Uid = table.Column<string>(type: "text", nullable: false),
                    Figi = table.Column<string>(type: "text", nullable: false),
                    Ticker = table.Column<string>(type: "text", nullable: false),
                    ClassCode = table.Column<string>(type: "text", nullable: false),
                    Isin = table.Column<string>(type: "text", nullable: false),
                    Sector = table.Column<string>(type: "text", nullable: false),
                    InstrumentType = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CountryOfRisk = table.Column<string>(type: "text", nullable: false),
                    LiquidityFlag = table.Column<bool>(type: "boolean", nullable: false),
                    isFavorite = table.Column<bool>(type: "boolean", nullable: false),
                    isLastPriceSubscribed = table.Column<bool>(type: "boolean", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Securities", x => x.Uid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Securities");
        }
    }
}
