namespace POC.Application.Features.Suppliers.DTOs
{
    public class CreateSupplierRequest
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Address { get; set; } = default!;
    }
}
