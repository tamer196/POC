using POC.Application.Features.Warehouses.DTOs;
using POC.Domain.Entities;

namespace POC.Application.Features.Warehouses.Helper
{
    public static class WarehouseMapping
    {
        public static WarehouseDto ToDto(this Warehouse warehouse)
        {
            return new WarehouseDto
            {
                Id = warehouse.Id,
                Code = warehouse.Code,
                Name = warehouse.Name,
                Location = warehouse.Location,
                IsActive = warehouse.IsActive,
                CreatedAtUtc = warehouse.CreatedAtUtc,
                UpdatedAtUtc = warehouse.UpdatedAtUtc
            };
        }
    }
}
