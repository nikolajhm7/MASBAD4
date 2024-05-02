using Handin4.Data;
using Handin4.Models;
using Handin4.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;

namespace Handin4.Controllers
{
    [Authorize(Policy = "ManagerOrHigher")]
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly myDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<IngredientsController> _logger;

        public IngredientsController(IHttpContextAccessor contextAccessor, myDbContext context, ILogger<IngredientsController> logger)
        {
            _context = context;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> GetIngredient(string name)
        {
            var ingredient = await _context.Ingredients
                .Include(i => i.IngredientAllergens)
                    .ThenInclude(ia => ia.Allergen) // Inkluder allergener
                .FirstOrDefaultAsync(i => i.Name == name);

            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(ingredient);
        }

        [HttpPost]
        public async Task<ActionResult> CreateIngredient(CreateIngredientDTO ingredientDto)
        {
            if (ingredientDto == null || ingredientDto.Quantity < 0) { return BadRequest("Quantity cannot be negative."); }

            var ingredient = new Ingredient
            {
                Quantity = ingredientDto.Quantity,
                Name = ingredientDto.Name,
            };



            foreach (var allergenName in ingredientDto.AllergenName)
            {
                var all = _context.Allergens.FirstOrDefault(a => a.Name == allergenName);
                if (all != null)
                {
                    _logger.LogInformation($"Opretter ikke allergenen {allergenName}, da den allerede eksisterer i databasen");
                }
                else
                {
                    var tempAl = new Allergen
                    {
                        Name = allergenName,
                    };

                    var ingredientAllergen = new IngredientAllergen
                    {
                        Ingredient = ingredient,
                        Allergen = tempAl,
                    };

                    tempAl.IngredientAllergens.Add(ingredientAllergen);

                    ingredient.IngredientAllergens.Add(ingredientAllergen);

                    _context.IngredientAllergens.Add(ingredientAllergen);
                    _context.Allergens.Add(tempAl);
                }
            }

            _context.Ingredients.Add(ingredient);



            await _context.SaveChangesAsync();

            var userName = _contextAccessor.HttpContext?.User?.Identity?.Name;

            var log = new LogEntry
            {
                Username = userName,
                TimeStamp = DateTime.UtcNow,
                OperationType = "Create"
            };

            _logger.LogInformation("Request: {@LogInfo}", new
            {
                TimeStamp = DateTime.UtcNow,
                Username = userName,
                OperationType = "Create"
            });

            return Ok($"{ingredientDto.Name} was added to the database with the quantity of {ingredientDto.Quantity}");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateIngredient(Ingredient ingredient)
        {
            if (!IngredientExists(ingredient.Name)) { return NotFound($"{ingredient.Name} does not exist in the database"); }

            if (ingredient.Quantity < 0) { return BadRequest("Quantity cannot be negative."); }

            _context.Entry(ingredient).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            var userName = _contextAccessor.HttpContext?.User?.Identity?.Name;

            var log = new LogEntry{
                Username = userName, 
                TimeStamp = DateTime.UtcNow,
                OperationType = "Update"
            };


            _logger.LogInformation("Request: {@LogInfo}", new
            {
                TimeStamp = DateTime.UtcNow,
                Username = userName,
                OperationType = "Update"
            });

            return Ok($"{ingredient.Name} quantity is now {ingredient.Quantity}");
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteIngredient(string name)
        {
            if (!IngredientExists(name)) { return NotFound($"{name} does not exist in the database"); }
            var ingredient = await _context.Ingredients.FindAsync(name);

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            var userName = _contextAccessor.HttpContext?.User?.Identity?.Name;

            var log = new LogEntry
            {
                Username = userName,
                TimeStamp = DateTime.UtcNow,
                OperationType = "Delete"
            };

            _logger.LogInformation("Request: {@LogInfo}", new
            {
                TimeStamp = DateTime.UtcNow,
                Username =userName,
                OperationType = "Delete"
            });

            return Ok($"{name} was removed from the database");
        }

        private bool IngredientExists(string name)
        {
            return _context.Ingredients.Any(e => e.Name == name);
        }
    }
}
