using Microsoft.EntityFrameworkCore;
using StockApi.Domain.Interfaces;
using StockApi.Infrastructure.Context;
using StockApi.Infrastructure.Repositories;

namespace StockApi.Infrastructure.Configuration
{
    public static class ServiceExtension
    {
        public static void ConfigureDbExtension(this IServiceCollection service, IConfiguration configuration)
        {
            try
            {
                // Database Connection Configuration
                var connectionString = configuration.GetConnectionString("StockApiDb");
                IServiceCollection serviceCollection = service.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Erro: Connection string is null - {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Erro: Invalide DbContext Configuration - {ex.Message}");
                throw;
            }

            service.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
