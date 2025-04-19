using Microsoft.EntityFrameworkCore;
using StockApi.Domain.Dtos.Commons;
using StockApi.Domain.Entities.Commons;
using StockApi.Domain.Interfaces.Queries;
using StockApi.Infrastructure.Context;
using StockApi.Response;
using System.Linq.Expressions;

namespace StockApi.Infrastructure.Repositories.Commons
{
    public class BasePaginationQuery<TBaseEntity, TDto> :
        BaseFilterQuery<TBaseEntity>,
        IPaginationQuery<TDto>,
        IFilterQuery<TBaseEntity>
        where TBaseEntity : BaseEntity
        where TDto : BaseDto, new()
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TBaseEntity> _dbSet;

        public BasePaginationQuery(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TBaseEntity>();
        }

        /// <summary>
        /// Returns records in a pagination structure, based on the filter
        /// </summary>
        /// <param name="pageNumber">The number of the page to retrieve (starting from 1).</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="filters">An object containing the filtering criteria for the query.</param>
        /// <returns>Returns a PaginationResponse with records that match the specified filters.</returns>
        public async Task<PaginationResponse<TDto>> Pagination(int pageNumber, int pageSize, Dictionary<string, string>? filters)
        {
            var query = _dbSet.AsQueryable();

            if (filters is not null)
            {
                query = this.Filter(query, filters);
            }

            var totalRecords = await query.CountAsync();
            var records = await ExecuteQuery(query, pageNumber, pageSize);
            return new PaginationResponse<TDto>(records, pageNumber, pageSize, totalRecords);
        }

        /// <summary>
        /// Convert the Entity to his DTO
        /// </summary>
        /// <returns>Lambda exression which convert Entity to DTO</returns>
        protected virtual Expression<Func<TBaseEntity, TDto>> ConvertToDto()
        {
            return x => (TDto)new TDto().ConvertToDto(x);
        }

        /// <summary>
        /// Executes a paginated query against the provided IQueryable source and converts the results to DTOs.
        /// </summary>
        /// <param name="query">The base query to be executed.</param>
        /// <param name="pageNumber">The number of the page to retrieve (starting from 1).</param>
        /// <param name="pageSize">The number of records to include per page.</param>
        /// <returns>A task that represents the asynchronous operation, containing a paginated list of DTOs.</returns>
        protected virtual async Task<List<TDto>> ExecuteQuery(IQueryable<TBaseEntity> query, int pageNumber, int pageSize)
        {
            return await query
                .AsNoTracking()
                .Select(ConvertToDto())
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
