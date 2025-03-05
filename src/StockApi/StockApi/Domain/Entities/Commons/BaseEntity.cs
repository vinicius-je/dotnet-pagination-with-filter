using System.ComponentModel.DataAnnotations;

namespace StockApi.Domain.Entities.Commons
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
