using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Handin4.Models
{
    public class Allergen
    {
        [Key]
        public string? Name { get; set; }
        public ICollection<IngredientAllergen> IngredientAllergens { get; set; } = new List<IngredientAllergen>();
    }
}
