using StockApi.Domain.Dtos;
using StockApi.Domain.Entities;
using StockApi.Domain.Interfaces;
using StockApi.Domain.Interfaces.Queries;
using StockApi.Infrastructure.Context;
using StockApi.Infrastructure.Repositories.Commons;
using StockApi.Response;

namespace StockApi.Infrastructure.Repositories
{
    public class ProductRepository :
        BasePaginationQuery<Product, ProductDto>,
        IPaginationQuery<ProductDto>,
        IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PaginationResponse<ProductDto>> GetProductsWithPaginationAndFilter(int pageNumber, int pageSize, Dictionary<string, string>? filters)
        {
            return await this.Pagination(pageNumber, pageSize, filters);
        }
    }
}
