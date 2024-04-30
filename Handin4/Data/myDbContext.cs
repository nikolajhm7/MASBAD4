using Handin4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Handin4.Data
{
    public partial class myDbContext : DbContext
    {
        public myDbContext() { }

        public myDbContext(DbContextOptions<myDbContext> options) : base(options) { }

        public DbSet<Supermarket> Supermarkets { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<SOrder> Orders { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Packet> Packets { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<IngredientAllergen> IngredientAllergens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "AspNetRoles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("AspNetUserRoles");
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("AspNetUserClaims");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("AspNetUserLogins");
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("AspNetUserTokens");
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("AspNetRoleClaims");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "AspNetUsers");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SOrder>()
                        .HasOne(o => o.Supermarket)
                        .WithMany()
                        .HasForeignKey(o => o.SupermarketId);


            modelBuilder.Entity<SOrder>()
                        .HasOne(o => o.Schedule)
                        .WithMany()
                        .HasForeignKey(o => o.ScheduleId);

            modelBuilder.Entity<Batch>()
                        .HasOne(b => b.Schedule)
                        .WithMany()
                        .HasForeignKey(b => b.ScheduleId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Batch>()
                        .HasOne(b => b.SOrder)
                        .WithMany()
                        .HasForeignKey(b => b.OrderId);

            modelBuilder.Entity<RecipeIngredient>()
                        .HasKey(ri => new { ri.RecipeId, ri.IngredientName });

            modelBuilder.Entity<RecipeIngredient>()
                        .HasOne(ri => ri.Recipe)
                        .WithMany(r => r.RecipeIngredients)
                        .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                        .HasOne(ri => ri.Ingredient)
                        .WithMany()
                        .HasForeignKey(ri => ri.IngredientName);

            modelBuilder.Entity<Packet>()
                        .HasOne(p => p.SOrder)
                        .WithMany()
                        .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<Packet>()
                        .HasOne(p => p.Driver)
                        .WithMany()
                        .HasForeignKey(p => p.DriverId);

            modelBuilder.Entity<IngredientAllergen>()
            .HasKey(ia => new { ia.IngredientName, ia.AllergenName });

            modelBuilder.Entity<IngredientAllergen>()
                        .HasOne(ia => ia.Ingredient)
                        .WithMany(i => i.IngredientAllergens)
                        .HasForeignKey(ia => ia.IngredientName);

            modelBuilder.Entity<IngredientAllergen>()
                        .HasOne(ia => ia.Allergen)
                        .WithMany(a => a.IngredientAllergens)
                        .HasForeignKey(ia => ia.AllergenName);
        }
    }
}
