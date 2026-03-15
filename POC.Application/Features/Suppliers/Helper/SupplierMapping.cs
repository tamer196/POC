using POC.Application.Features.Suppliers.DTOs;
using POC.Domain.Entities;

namespace POC.Application.Features.Suppliers.Helper
{
    public static class SupplierMapping
    {
        public static SupplierDto ToDto(this Supplier supplier)
        {
            return new SupplierDto
            {
                Id = supplier.Id,
                Code = supplier.Code,
                Name = supplier.Name,
                Email = supplier.Email,
                Phone = supplier.Phone,
                Address = supplier.Address,
                IsActive = supplier.IsActive,
                CreatedAtUtc = supplier.CreatedAtUtc,
                UpdatedAtUtc = supplier.UpdatedAtUtc
            };
        }
    }
}
