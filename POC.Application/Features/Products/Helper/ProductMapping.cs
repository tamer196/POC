using POC.Application.Features.Products.DTOs;
using POC.Domain.Entities;

namespace POC.Application.Features.Products.Helper
{
    public static class ProductMapping
    {
        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                SKU = product.SKU,
                Name = product.Name,
                Description = product.Description,
                CostPrice = product.CostPrice,
                SellingPrice = product.SellingPrice,
                QuantityInStock = product.QuantityInStock,
                Status = product.Status,
                SupplierId = product.SupplierId,
                WarehouseId = product.WarehouseId,
                CreatedAtUtc = product.CreatedAtUtc,
                UpdatedAtUtc = product.UpdatedAtUtc
            };
        }
    }
}
