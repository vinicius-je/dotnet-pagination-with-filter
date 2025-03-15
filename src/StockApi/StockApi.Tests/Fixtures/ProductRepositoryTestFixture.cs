using Microsoft.EntityFrameworkCore;
using StockApi.Domain.Entities;
using StockApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApi.Tests.Fixtures
{
    public class ProductRepositoryTestFixture : IDisposable
    {
        public AppDbContext Context { get; private set; }

        public ProductRepositoryTestFixture(DbContextOptions<AppDbContext> options)
        {
            Context = new AppDbContext(options);

            // Seed data
            Context.Products.AddRange(
                new Product { Id = new Guid(), Name = "Product 1", Price = 10.50m, Category = ProductCategoryEnum.Electronics, Quantity = 5 },
                new Product { Id = new Guid(), Name = "Product 2", Price = 20.75m, Category = ProductCategoryEnum.Electronics, Quantity = 10 },
                new Product { Id = new Guid(), Name = "Product 3", Price = 15.99m, Category = ProductCategoryEnum.Clothing, Quantity = 15 },
                new Product { Id = new Guid(), Name = "Product 4", Price = 25.99m, Category = ProductCategoryEnum.Clothing, Quantity = 8 },
                new Product { Id = new Guid(), Name = "Product 5", Price = 5.99m, Category = ProductCategoryEnum.Toys, Quantity = 20 }
            );
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
