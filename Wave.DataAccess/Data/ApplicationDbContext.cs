using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ExceptionServices;
using Wave.Models;

namespace Wave.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "First",
                    DisplayOrder = 1
                },
                new Category
                {
                    Id = 2,
                    Name = "Second",
                    DisplayOrder = 2,
                },
                new Category
                {
                    Id = 3,
                    Name = "Third",
                    DisplayOrder = 3,
                }
                );

            modelBuilder.Entity<Product>().HasData(
                    new Product
                    {
                        Id = 9,
                        Name = "SILIKON UV SPEED SCHWIMMBRILLE \"ANTI-FOG\"",
                        Description = "Die bequeme, verspiegelte Unisex Schwimmbrille aus Silikon von wave-plus ist perfekt für das Training im Wasser. Die Dichtungen passen sich dem Gesicht an, reduzieren den Druck um die Augen und sorgen für ein leichtes Tragegefühl. Die breiten Gläser gewährleisten eine optimale seitliche Sicht. Das elastische Kopfband lässt sich gut am Hinterkopf dem individuellen Kopfumfang anpassen und garantiert einen sehr stabilen Sitz während dem Training. Die Anti-Fog Beschichtung der Gläser schützt vor Beschlagen.",
                        ListPrice = 16.90,
                        Price = 16.90,
                        CategoryId = 1,
                    },
                    new Product
                    {
                        Id = 7,
                        Name = "SILIKON Klare SCHWIMMBRILLE \"ANTI-FOG\" ( Schwarz,Waiss,Blau)",
                        Description = "Die bequeme, verspiegelte Unisex Schwimmbrille aus Silikon von wave-plus ist perfekt für das Training im Wasser. Die Dichtungen passen sich dem Gesicht an, reduzieren den Druck um die Augen und sorgen für ein leichtes Tragegefühl. Die breiten Gläser gewährleisten eine optimale seitliche Sicht. Das elastische Kopfband lässt sich gut am Hinterkopf dem individuellen Kopfumfang anpassen und garantiert einen sehr stabilen Sitz während dem Training. Die Anti-Fog Beschichtung der Gläser schützt vor Beschlagen.",
                        ListPrice = 16.90,
                        Price = 16.90,
                        CategoryId = 2,
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "SILIKON KINDER SCHWIMMBRILLE \"ANTI-FOG\" (Königsblau, Hellblau, Gelb-Rosa)",
                        Description = "Die komfortable Silikon Kinder Schwimmbrille  wave-plus ist perfekt für die Kleinen Wasserratten und es gibt sie in folgenden Farben: Königsblau, Blau und Gelb-Rosa. Die weichen Augenschalen passen sich dem Gesicht an und reduzieren den Druck um die Augen. Das elastische Kopfband lässt sich gut am Hinterkopf anpassen und garantiert einen sehr stabilen Sitz während dem Training. Die Anti-Fog Beschichtung der Gläser schützt vor Beschlagen.",
                        ListPrice = 12.90,
                        Price = 12.90,
                        CategoryId = 3,
                    }
                );
        }
    }
}
