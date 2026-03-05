using System;
using System.Collections.Generic;
using System.Data.Entity;
using LegacyInventory.Models;

namespace LegacyInventory.Data
{
    // DropCreateDatabaseIfModelChanges:
    //   - Creates the DB on first run if it does not exist
    //   - Drops and recreates if the EF model has changed
    //   - Seeds sample data after creation
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<InventoryDbContext>
    {
        protected override void Seed(InventoryDbContext context)
        {
            var categories = new List<Category>
            {
                new Category { Name = "Electronics",  Description = "Electronic devices and accessories" },
                new Category { Name = "Furniture",    Description = "Home and office furniture" },
                new Category { Name = "Clothing",     Description = "Apparel and fashion items" },
            };
            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product
                {
                    Name        = "Laptop Pro 15",
                    Description = "High-performance business laptop with 16GB RAM",
                    Price       = 1299.99m,
                    Stock       = 25,
                    CategoryId  = 1,
                    CreatedAt   = DateTime.Now,
                },
                new Product
                {
                    Name        = "Wireless Mouse",
                    Description = "Ergonomic wireless optical mouse",
                    Price       = 29.99m,
                    Stock       = 100,
                    CategoryId  = 1,
                    CreatedAt   = DateTime.Now,
                },
                new Product
                {
                    Name        = "Office Chair",
                    Description = "Adjustable ergonomic office chair with lumbar support",
                    Price       = 349.00m,
                    Stock       = 15,
                    CategoryId  = 2,
                    CreatedAt   = DateTime.Now,
                },
                new Product
                {
                    Name        = "Standing Desk",
                    Description = "Height-adjustable standing desk 140x70cm",
                    Price       = 599.00m,
                    Stock       = 8,
                    CategoryId  = 2,
                    CreatedAt   = DateTime.Now,
                },
                new Product
                {
                    Name        = "T-Shirt Basic",
                    Description = "100% cotton crew-neck basic t-shirt",
                    Price       = 19.99m,
                    Stock       = 200,
                    CategoryId  = 3,
                    CreatedAt   = DateTime.Now,
                },
            };
            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();
        }
    }
}
