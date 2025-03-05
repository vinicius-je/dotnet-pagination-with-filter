using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockApi.Domain.Entities;

namespace StockApi.Infrastructure.EntitiesConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            var converter = new ValueConverter<ProductCategoryEnum, int>(
                v => (int)v,
                v => (ProductCategoryEnum)v);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Price).HasPrecision(18, 2).IsRequired();

            builder.Property(x => x.Category)
                .HasConversion(converter)
                .IsRequired();
        }
    }
}
