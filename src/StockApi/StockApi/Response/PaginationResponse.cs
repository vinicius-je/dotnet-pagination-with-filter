namespace StockApi.Response
{
    public class PaginationResponse<T>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalRecords { get; init; }
        public int TotalPages { get; init; }
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
        public List<T> Data { get; init; }

        public PaginationResponse(List<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = CalculeTotalPages(totalRecords, pageSize);
        }

        public int CalculeTotalPages(int totalRecords, int pageSize)
        {
            return totalRecords == 0 ? totalRecords : (int)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
        }
    }
}
