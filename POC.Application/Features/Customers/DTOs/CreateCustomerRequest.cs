namespace POC.Application.Features.Customers.DTOs
{
    public class CreateCustomerRequest
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Address { get; set; } = default!;
    }
}
