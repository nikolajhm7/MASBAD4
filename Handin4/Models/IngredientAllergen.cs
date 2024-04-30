namespace Handin4.Models
{
    public class IngredientAllergen
    {
        public string? IngredientName { get; set; }
        public Ingredient? Ingredient { get; set; }

        public string? AllergenName { get; set; }
        public Allergen? Allergen { get; set; }
    }
}
