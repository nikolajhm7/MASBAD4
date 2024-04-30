using Handin4.Data;
using Handin4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Handin4.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly myDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedController(myDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #region SeedData
        [HttpPost]
        public IActionResult SeedData()
        {
            try
            {
                // Inserting data into Supermarket table
                _context.Supermarkets.AddRange(
                    new Supermarket { Name = "Føtex", Location = "Finlandsgade 17, 8200 Aarhus N", Latitude = 56.171089, Longitude = 10.190290 },
                    new Supermarket { Name = "REMA 1000", Location = "Randersvej 142, 8200 Aarhus N", Latitude = 56.178930, Longitude = 10.200730 }
                );

                _context.SaveChanges();

                // Inserting data into Schedule table
                _context.Schedules.Add(new Schedule { Name = "BakerySchedule" });

                _context.SaveChanges();

                // Inserting data into SOrder table
                _context.Orders.AddRange(
                    new SOrder { SupermarketId = 1, ScheduleId = 1, Date = "05-03-2024", Location = "Finlandsgade 17, 8200 Aarhus N" },
                    new SOrder { SupermarketId = 2, ScheduleId = 1, Date = "03-02-2023", Location = "Randersvej 142, 8200 Aarhus N" },
                    new SOrder { SupermarketId = 1, ScheduleId = 1, Date = "12-10-2021", Location = "Finlandsgade 17, 8200 Aarhus N" }
                );

                _context.SaveChanges();

                // Inserting data into Batch table
                _context.Batches.AddRange(
                    new Batch { ScheduleId = 1, OrderId = 1, Quantity = 400, StartTime = "12-10-2021 10:00", TargetFinishTime = "12-10-2021 16:00", ActualFinishTime = "12-10-2021 16:30", RecipeId = 3 },
                    new Batch { ScheduleId = 1, OrderId = 1, Quantity = 300, StartTime = "12-10-2021 10:00", TargetFinishTime = "12-10-2021 16:00", ActualFinishTime = "12-10-2021 15:50", RecipeId = 1 },
                    new Batch { ScheduleId = 1, OrderId = 1, Quantity = 300, StartTime = "12-10-2021 10:00", TargetFinishTime = "12-10-2021 16:00", ActualFinishTime = "12-10-2021 17:00", RecipeId = 2 },
                    new Batch { ScheduleId = 1, OrderId = 2, Quantity = 700, StartTime = "03-02-2023 10:00", TargetFinishTime = "03-02-2023 16:00", ActualFinishTime = "03-02-2023 15:45", RecipeId = 3 },
                    new Batch { ScheduleId = 1, OrderId = 2, Quantity = 100, StartTime = "03-02-2023 10:00", TargetFinishTime = "03-02-2023 16:00", ActualFinishTime = "03-02-2023 16:00", RecipeId = 2 },
                    new Batch { ScheduleId = 1, OrderId = 3, Quantity = 300, StartTime = "05-03-2024 10:00", TargetFinishTime = "05-03-2024 16:00", ActualFinishTime = "05-03-2024 18:00", RecipeId = 2 },
                    new Batch { ScheduleId = 1, OrderId = 3, Quantity = 200, StartTime = "05-03-2024 10:00", TargetFinishTime = "05-03-2024 16:00", ActualFinishTime = "05-03-2024 15:30", RecipeId = 1 }
                );

                _context.SaveChanges();

                // Inserting data into Recipe table
                _context.Recipes.AddRange(
                    new Recipe { Name = "Alexandertorte" },
                    new Recipe { Name = "Butter cookies" },
                    new Recipe { Name = "Studenterbrød" },
                    new Recipe { Name = "Romkugler" }
                );

                _context.SaveChanges();

                // Inserting data into Ingredient table
                _context.Ingredients.AddRange(
                    new Ingredient { Name = "Sugar", Quantity = 100000 },
                    new Ingredient { Name = "Flour", Quantity = 242000 },
                    new Ingredient { Name = "Salt", Quantity = 10000 },
                    new Ingredient { Name = "Rum", Quantity = 2000 },
                    new Ingredient { Name = "Cocoa", Quantity = 3000 },
                    new Ingredient { Name = "Raspberry jam", Quantity = 5000 },
                    new Ingredient { Name = "Leftover cake", Quantity = 31415 }
                );

                _context.SaveChanges();

                // Inserting data into RecipeIngredient table
                _context.RecipeIngredients.AddRange(
                    new RecipeIngredient { RecipeId = 1, IngredientName = "Sugar", Quantity = 300 },
                    new RecipeIngredient { RecipeId = 1, IngredientName = "Cocoa", Quantity = 20 },
                    new RecipeIngredient { RecipeId = 1, IngredientName = "Raspberry jam", Quantity = 50 },
                    new RecipeIngredient { RecipeId = 1, IngredientName = "Leftover Cake", Quantity = 30 },
                    new RecipeIngredient { RecipeId = 2, IngredientName = "Sugar", Quantity = 100 },
                    new RecipeIngredient { RecipeId = 2, IngredientName = "Flour", Quantity = 200 },
                    new RecipeIngredient { RecipeId = 2, IngredientName = "Salt", Quantity = 20 },
                    new RecipeIngredient { RecipeId = 2, IngredientName = "Leftover Cake", Quantity = 305 },
                    new RecipeIngredient { RecipeId = 3, IngredientName = "Sugar", Quantity = 320 },
                    new RecipeIngredient { RecipeId = 3, IngredientName = "Cocoa", Quantity = 20 },
                    new RecipeIngredient { RecipeId = 3, IngredientName = "Rum", Quantity = 50 },
                    new RecipeIngredient { RecipeId = 3, IngredientName = "Leftover Cake", Quantity = 30 }
                );

                _context.SaveChanges();

                // Inserting data into Driver table
                _context.Drivers.Add(new Driver { Name = "Star" });

                _context.SaveChanges();

                // Inserting data into Packet table
                _context.Packets.AddRange(
                    new Packet { OrderId = 1, DriverId = 1 },
                    new Packet { OrderId = 2, DriverId = 1 }
                );

                _context.SaveChanges();

                return Ok("Data seeded successfully");
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                return StatusCode(500, $"Error while seeding data: {ex.Message}");
            }
        }
        #endregion
        #region SeedAllergens
        [HttpPost("SeedAllergens")]
        public IActionResult SeedAllergens()
        {
            try
            {
                // Inserting data into Allergen table
                _context.Allergens.AddRange(
                    new Allergen { Name = "Gluten" },
                    new Allergen { Name = "Lactose" },
                    new Allergen { Name = "Nuts" }
                );

                _context.SaveChanges();

                // Inserting data into IngredientAllergen table
                if (!_context.IngredientAllergens.Any(ia => ia.IngredientName == "Leftover Cake"))
                {
                    _context.IngredientAllergens.AddRange(
                        new IngredientAllergen { IngredientName = "Leftover Cake", AllergenName = "Gluten" },
                        new IngredientAllergen { IngredientName = "Leftover Cake", AllergenName = "Lactose" },
                        new IngredientAllergen { IngredientName = "Leftover Cake", AllergenName = "Nuts" }
                    );

                    _context.SaveChanges();
                }

                return Ok("Allergens seeded successfully");
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                return StatusCode(500, $"Error while seeding allergens: {ex.Message}");
            }
        }
        #endregion
        #region SeedRoles
        [HttpPost("SeedRoles")]
        public async Task<IActionResult> SeedRoles()
        {
            var seededRoles = 0;

            var rolesToCreate = new List<string> { "Admin", "Manager", "Baker", "Driver" };

            foreach (var roleName in rolesToCreate)
            {
                // Kontroller om rollen allerede findes
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    // Opret rollen
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                    if (roleResult.Succeeded)
                    {
                        seededRoles++;
                    }
                }
            }

            return Ok($"Seeded roles: {seededRoles}");
        }
        #endregion
        #region SeedUsers
        [HttpPut("SeedUsers")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> SeedUsers()
        {
            var seededUsers = 0;



            const string adminEmail = "admin@example.com";
            const string genericPassword = "YourStrongPassword1!";

            if (await _userManager.FindByNameAsync(adminEmail) == null)
            {
                var adminUser = new User();
                adminUser.Name = "Admin Adminsen";
                adminUser.UserName = adminEmail;
                adminUser.Email = adminEmail;
                adminUser.EmailConfirmed = true;
                IdentityResult result = _userManager.CreateAsync(adminUser, genericPassword).Result;
                if (result.Succeeded)
                {
                    var newAdminUser = _userManager.FindByNameAsync(adminEmail).Result;
                    var adminClaim = new Claim(ClaimTypes.Role, "Admin");
                    var claimAdded = _userManager.AddClaimAsync(newAdminUser, adminClaim).Result;
                    seededUsers++;
                }
            }

            const string managerEmail = "manager@example.com";


            if (await _userManager.FindByNameAsync(managerEmail) == null)
            {
                var managerUser = new User();
                managerUser.Name = "Manager Managersen";
                managerUser.UserName = managerEmail;
                managerUser.Email = managerEmail;
                managerUser.EmailConfirmed = true;
                IdentityResult result = _userManager.CreateAsync(managerUser, genericPassword).Result;
                if (result.Succeeded)
                {
                    var newManagerUser = _userManager.FindByNameAsync(managerEmail).Result;
                    var managerClaim = new Claim(ClaimTypes.Role, "Manager");
                    var claimAdded = _userManager.AddClaimAsync(newManagerUser, managerClaim).Result;
                    seededUsers++;
                }
            }

            const string bakerEmail = "baker@example.com";

            if (await _userManager.FindByNameAsync(bakerEmail) == null)
            {
                var bakerUser = new User();
                bakerUser.Name = "Baker Bakersen";
                bakerUser.UserName = bakerEmail;
                bakerUser.Email = bakerEmail;
                bakerUser.EmailConfirmed = true;
                IdentityResult result = _userManager.CreateAsync(bakerUser, genericPassword).Result;
                if (result.Succeeded)
                {
                    var newBakerUser = _userManager.FindByNameAsync(bakerEmail).Result;
                    var bakerClaim = new Claim(ClaimTypes.Role, "Baker");
                    var claimAdded = _userManager.AddClaimAsync(newBakerUser, bakerClaim).Result;
                    seededUsers++;
                }
            }

            const string driverEmail = "driver@example.com";

            if (await _userManager.FindByNameAsync(driverEmail) == null)
            {
                var driverUser = new User();
                driverUser.Name = "Driver Driversen";
                driverUser.UserName = driverEmail;
                driverUser.Email = driverEmail;
                driverUser.EmailConfirmed = true;
                IdentityResult result = _userManager.CreateAsync(driverUser, genericPassword).Result;
                if (result.Succeeded)
                {
                    var newDriverUser = _userManager.FindByNameAsync(driverEmail).Result;
                    var driverClaim = new Claim(ClaimTypes.Role, "Driver");
                    var claimAdded = _userManager.AddClaimAsync(newDriverUser, driverClaim).Result;
                    seededUsers++;
                }
            }

            return Ok(new { SeededUsers = seededUsers });
        }


        #endregion
    }
}
