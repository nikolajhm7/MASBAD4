using Handin4.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Handin4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly myDbContext _context;
        private readonly IConfiguration _configuration;

        public QueryController(myDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Query 1: Get all ingredients
        [HttpGet("Ingredients")]
        [Authorize(Policy = "BakerOrHigher")]
        public async Task<ActionResult> GetIngredients()
        {
            var ingredients = await _context.Ingredients
                .Select(i => new { IngredientName = i.Name, Quantity = i.Quantity })
                .ToListAsync();

            return Ok(ingredients);
        }

        // Query 2: Get order information by orderId
        [Authorize(Policy = "DriverOrHigher")]
        [HttpGet("Order/{orderId}")]
        public async Task<ActionResult> GetOrder(int orderId)
        {
            var orderInfo = await _context.Orders
                .Where(o => o.OrderId == orderId)
                .Select(o => new { Address = o.Location, OrderDate = o.Date })
                .FirstOrDefaultAsync();

            if (orderInfo == null)
            {
                return NotFound();
            }

            return Ok(orderInfo);
        }

        // Query 3: Get baked goods information by orderId
        [Authorize(Policy = "DriverOrHigher")]
        [HttpGet("BakedGoods/{orderId}")]
        public async Task<ActionResult> GetBakedGoods(int orderId)
        {
            var bakedGoods = await _context.Batches
                .Where(b => b.OrderId == orderId)
                .Join(_context.Recipes,
                    b => b.RecipeId,
                    r => r.RecipeId,
                    (b, r) => new { BakedGood = r.Name, Quantity = b.Quantity })
                .ToListAsync();

            return Ok(bakedGoods);
        }

        // Query 4: Get ingredient information for a batch
        [Authorize(Policy = "BakerOrHigher")]
        [HttpGet("BatchIngredients/{batchId}")]
        public async Task<ActionResult> GetBatchIngredients(int batchId)
        {
            var batchIngredients = await _context.RecipeIngredients
                .Where(ri => _context.Batches.Any(b => b.RecipeId == ri.RecipeId && b.BatchId == batchId))
                .Select(ri => new
                {
                    Ingredient = ri.IngredientName,
                    TotalQuantity = ri.Quantity,
                    Allergens = _context.IngredientAllergens
                                .Where(ia => ia.IngredientName == ri.IngredientName)
                                .Select(ia => ia.AllergenName)
                                .ToList()
                })
                .ToListAsync();

            return Ok(batchIngredients);
        }

        // Query 5: Get trackId for an order
        [Authorize(Policy = "DriverOrHigher")]
        [HttpGet("TrackId/{orderId}")]
        public async Task<ActionResult> GetTrackId(int orderId)
        {
            var trackInfo = await _context.Packets
                .Where(p => p.OrderId == orderId)
                .Join(
                    _context.Orders,
                    p => p.OrderId,
                    o => o.OrderId,
                    (p, o) => new { TrackId = p.TrackId, Address = o.Supermarket.Location ,Latitude = o.Supermarket.Latitude, Longitude = o.Supermarket.Longitude }
                )
                .FirstOrDefaultAsync();

            if (trackInfo == null)
            {
                return NotFound();
            }

            return Ok(trackInfo);
        }

        // Query 6: Get total quantity for each baked good
        [Authorize(Policy = "ManagerOrHigher")]
        [HttpGet("TotalBakedGoods")]
        public async Task<ActionResult> GetTotalBakedGoods()
        {
            var totalBakedGoods = await _context.Batches
                .Join(_context.Recipes,
                    b => b.RecipeId,
                    r => r.RecipeId,
                    (b, r) => new { BakedGood = r.Name, Quantity = b.Quantity })
                .GroupBy(x => x.BakedGood)
                .Select(g => new { BakedGood = g.Key, TotalQuantity = g.Sum(x => x.Quantity) })
                .OrderBy(x => x.BakedGood)
                .ToListAsync();

            return Ok(totalBakedGoods);
        }

        // Query 7: Get average delay for all batches
        [Authorize(Policy = "BakerOrHigher")]
        [HttpGet("AverageDelay")]
        public async Task<ActionResult> GetAverageDelay()
        {
            var avgDelay = await CalculateAverageDelayAsync();

            return Ok(avgDelay);
        }

        private async Task<double> CalculateAverageDelayAsync()
        {
            var batchesWithValidTimes = await _context.Batches
                .Where(b => b.TargetFinishTime != null && b.ActualFinishTime != null)
                .ToListAsync();

            var delaysInMinutes = batchesWithValidTimes
                .Select(b => DateTime.ParseExact(b.ActualFinishTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture) -
                             DateTime.ParseExact(b.TargetFinishTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture))
                .Select(ts => ts.TotalMinutes);

            double avgDelay = delaysInMinutes.Average();

            return avgDelay;
        }

    }
}
