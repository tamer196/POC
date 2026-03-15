using POC.Domain.Enums;

namespace POC.Application.Features.Products.DTOs
{
    public class UpdateProductRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public ProductStatus Status { get; set; }
        public Guid SupplierId { get; set; }
        public Guid WarehouseId { get; set; }
    }
}
