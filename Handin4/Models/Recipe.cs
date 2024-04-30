using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Handin4.Models
{
    public class Recipe
    {
        [Key]
        public long RecipeId { get; set; }
        public string? Name { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
