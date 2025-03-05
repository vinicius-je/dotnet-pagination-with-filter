namespace StockApi.Domain.Interfaces.Queries
{
    public interface IFilterQuery<T>
    {
        public IQueryable<T> Filter(IQueryable<T> query, Dictionary<string, string> filters);
    }
}
