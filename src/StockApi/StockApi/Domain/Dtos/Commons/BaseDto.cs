using StockApi.Domain.Entities.Commons;

namespace StockApi.Domain.Dtos.Commons
{
    public record BaseDto
    {
        public Guid Id { get; set; }

        public virtual object ConvertToDto(BaseEntity entity)
        {
            Id = entity.Id;
            return this;
        }
    }
}
