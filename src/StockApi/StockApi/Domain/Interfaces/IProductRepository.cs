using StockApi.Domain.Dtos;
using StockApi.Response;

namespace StockApi.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<PaginationResponse<ProductDto>> GetProductsWithPaginationAndFilter(int pageNumber, int pageSize, Dictionary<string, string>? filters);

    }
}
