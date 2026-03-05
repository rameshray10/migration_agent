using System.Data.Entity;
using LegacyInventory.Models;

namespace LegacyInventory.Data
{
    // EF6 Code-First DbContext
    // Connection string name matches Web.config: name=InventoryDB
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext() : base("name=InventoryDB")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Set decimal precision for Price column
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal")
                .HasPrecision(18, 2);
        }
    }
}
