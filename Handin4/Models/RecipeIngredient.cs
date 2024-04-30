namespace Handin4.Models
{
    public class RecipeIngredient
    {
        public long RecipeId { get; set; }
        public string? IngredientName { get; set; }
        public long Quantity { get; set; }
        public Recipe? Recipe { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
