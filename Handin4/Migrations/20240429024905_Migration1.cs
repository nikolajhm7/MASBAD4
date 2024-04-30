using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Handin4.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allergens",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergens", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "IngredientAllergens",
                columns: table => new
                {
                    IngredientName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AllergenName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientAllergens", x => new { x.IngredientName, x.AllergenName });
                    table.ForeignKey(
                        name: "FK_IngredientAllergens_Allergens_AllergenName",
                        column: x => x.AllergenName,
                        principalTable: "Allergens",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientAllergens_Ingredients_IngredientName",
                        column: x => x.IngredientName,
                        principalTable: "Ingredients",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientAllergens_AllergenName",
                table: "IngredientAllergens",
                column: "AllergenName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientAllergens");

            migrationBuilder.DropTable(
                name: "Allergens");
        }
    }
}
