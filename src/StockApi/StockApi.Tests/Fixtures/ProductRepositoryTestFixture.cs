using Microsoft.EntityFrameworkCore;
using StockApi.Domain.Entities;
using StockApi.Infrastructure.Context;

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
                new Product { Id = new Guid(), Name = "Product 1", Price = decimal.Parse("10.50"), Category = ProductCategoryEnum.Electronics, Quantity = 5 },
                new Product { Id = new Guid(), Name = "Product 2", Price = decimal.Parse("20.75"), Category = ProductCategoryEnum.Electronics, Quantity = 10 },
                new Product { Id = new Guid(), Name = "Product 3", Price = decimal.Parse("15.50"), Category = ProductCategoryEnum.Clothing, Quantity = 15 },
                new Product { Id = new Guid(), Name = "Product 4", Price = decimal.Parse("25.50"), Category = ProductCategoryEnum.Clothing, Quantity = 8 },
                new Product { Id = new Guid(), Name = "Product 5", Price = decimal.Parse("5.99"), Category = ProductCategoryEnum.Toys, Quantity = 20 }
            );
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
