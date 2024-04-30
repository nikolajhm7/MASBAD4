using System.ComponentModel.DataAnnotations;

namespace Handin4.Models
{
    public class Ingredient
    {
        [Key]
        public string? Name { get; set; }
        public long Quantity { get; set; }
        public ICollection<IngredientAllergen> IngredientAllergens { get; set; } = new List<IngredientAllergen>();
    }
}
