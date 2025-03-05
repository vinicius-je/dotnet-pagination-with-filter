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

        public async Task<PaginationResponse<TDto>> Pagination(int pageNumber, int pageSize, Dictionary<string, string>? filters)
        {
            var query = _dbSet.AsQueryable();

            if (filters is not null)
            {
                query = this.Filter(query, filters);
            }

            var totalRecords = await query.CountAsync();

            var products = await query
                .AsNoTracking()
                .Select(MapToProductDto())
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResponse<TDto>(products, pageNumber, pageSize, totalRecords);
        }

        private Expression<Func<TBaseEntity, TDto>> MapToProductDto()
        {
            return x => (TDto)new TDto().ConvertToDto(x);
        }
    }
}
