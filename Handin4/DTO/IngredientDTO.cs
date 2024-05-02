using Handin4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Handin4.DTO
{
    public class CreateIngredientDTO
    {
        public string? Name { get; set; }
        public long Quantity { get; set; }
        public List<string>? AllergenName { get; set; }

    }
}
