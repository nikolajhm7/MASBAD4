using Handin4.Data;
using Handin4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Handin4.Controllers
{
    [Authorize(Policy = "ManagerOrHigher")]
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly myDbContext _context;

        public IngredientsController(myDbContext context)
        {
            _context = context;
        }

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult> GetIngredients()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            return Ok(ingredients);
        }

        // GET: api/Ingredients/Flour
        [HttpGet("{name}")]
        public async Task<ActionResult> GetIngredient(string name)
        {
            var ingredient = await _context.Ingredients.FindAsync(name);

            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(ingredient);
        }

        // POST: api/Ingredients
        [HttpPost]
        public async Task<ActionResult> CreateIngredient(Ingredient ingredient)
        {
            if (ingredient == null || ingredient.Quantity < 0) { return BadRequest("Quantity cannot be negative."); }

            _context.Ingredients.Add(ingredient);

            await _context.SaveChangesAsync();

            return Ok($"{ingredient.Name} was added to the database with the quantity of {ingredient.Quantity}");
        }

        // PUT: api/Ingredients/Flour
        [HttpPut]
        public async Task<IActionResult> UpdateIngredient(Ingredient ingredient)
        {
            if (!IngredientExists(ingredient.Name)) { return NotFound($"{ingredient.Name} does not exist in the database"); }

            if (ingredient.Quantity < 0) { return BadRequest("Quantity cannot be negative."); }

            _context.Entry(ingredient).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok($"{ingredient.Name} quantity is now {ingredient.Quantity}");
        }

        // DELETE: api/Ingredients/Flour
        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteIngredient(string name)
        {
            if (!IngredientExists(name)) { return NotFound($"{name} does not exist in the database"); }
            var ingredient = await _context.Ingredients.FindAsync(name);

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return Ok($"{name} was removed from the database");
        }

        private bool IngredientExists(string name)
        {
            return _context.Ingredients.Any(e => e.Name == name);
        }
    }
}
