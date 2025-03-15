using Microsoft.EntityFrameworkCore;
using StockApi.Domain.Entities;

namespace StockApi.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public DbSet<Product> Products { get; set; }
    }
}
