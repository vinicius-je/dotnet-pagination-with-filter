using StockApi.Domain.Entities.Commons;

namespace StockApi.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ProductCategoryEnum Category { get; set; }
        public int Quantity { get; set; }

        public Product()
        {
        }

        public Product(string name, string description, decimal price, ProductCategoryEnum category, int quantity)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            Quantity = quantity;
        }
    }
}
