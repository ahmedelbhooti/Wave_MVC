using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Wave.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ListPrice", "Name", "Price" },
                values: new object[,]
                {
                    { 5, "Die komfortable Silikon Kinder Schwimmbrille  wave-plus ist perfekt für die Kleinen Wasserratten und es gibt sie in folgenden Farben: Königsblau, Blau und Gelb-Rosa. Die weichen Augenschalen passen sich dem Gesicht an und reduzieren den Druck um die Augen. Das elastische Kopfband lässt sich gut am Hinterkopf anpassen und garantiert einen sehr stabilen Sitz während dem Training. Die Anti-Fog Beschichtung der Gläser schützt vor Beschlagen.", 12.9, "SILIKON KINDER SCHWIMMBRILLE \"ANTI-FOG\" (Königsblau, Hellblau, Gelb-Rosa)", 12.9 },
                    { 7, "Die bequeme, verspiegelte Unisex Schwimmbrille aus Silikon von wave-plus ist perfekt für das Training im Wasser. Die Dichtungen passen sich dem Gesicht an, reduzieren den Druck um die Augen und sorgen für ein leichtes Tragegefühl. Die breiten Gläser gewährleisten eine optimale seitliche Sicht. Das elastische Kopfband lässt sich gut am Hinterkopf dem individuellen Kopfumfang anpassen und garantiert einen sehr stabilen Sitz während dem Training. Die Anti-Fog Beschichtung der Gläser schützt vor Beschlagen.", 16.899999999999999, "SILIKON Klare SCHWIMMBRILLE \"ANTI-FOG\" ( Schwarz,Waiss,Blau)", 16.899999999999999 },
                    { 9, "Die bequeme, verspiegelte Unisex Schwimmbrille aus Silikon von wave-plus ist perfekt für das Training im Wasser. Die Dichtungen passen sich dem Gesicht an, reduzieren den Druck um die Augen und sorgen für ein leichtes Tragegefühl. Die breiten Gläser gewährleisten eine optimale seitliche Sicht. Das elastische Kopfband lässt sich gut am Hinterkopf dem individuellen Kopfumfang anpassen und garantiert einen sehr stabilen Sitz während dem Training. Die Anti-Fog Beschichtung der Gläser schützt vor Beschlagen.", 16.899999999999999, "SILIKON UV SPEED SCHWIMMBRILLE \"ANTI-FOG\"", 16.899999999999999 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
