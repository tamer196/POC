using POC.Application.Features.Customers.DTOs;
using POC.Domain.Entities;

namespace POC.Application.Features.Customers.Helper
{
    public static class CustomerMappings
    {
        public static CustomerDto ToDto(this Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                Code = customer.Code,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                IsActive = customer.IsActive,
                CreatedAtUtc = customer.CreatedAtUtc,
                UpdatedAtUtc = customer.UpdatedAtUtc
            };
        }
    }
}
