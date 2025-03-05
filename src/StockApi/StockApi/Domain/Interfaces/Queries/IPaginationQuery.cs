using StockApi.Response;

namespace StockApi.Domain.Interfaces.Queries
{
    public interface IPaginationQuery<T>
    {
        Task<PaginationResponse<T>> Pagination(int pageNumber, int pageSize, Dictionary<string, string>? filters);
    }
}
