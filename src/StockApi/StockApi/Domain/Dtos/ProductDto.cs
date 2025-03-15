using StockApi.Domain.Dtos.Commons;
using StockApi.Domain.Entities;
using StockApi.Domain.Entities.Commons;

namespace StockApi.Domain.Dtos
{
    public record ProductDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public override object ConvertToDto(BaseEntity entity)
        {
            var p = (Product)entity;

            Id = p.Id;
            Name = p.Name;
            Category = p.Category.ToString();
            Description = p.Description;
            Price = p.Price.ToString("F2");
            Quantity = p.Quantity;

            return this;
        }
    }
}
