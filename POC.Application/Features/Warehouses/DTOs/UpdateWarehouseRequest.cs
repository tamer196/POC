namespace POC.Application.Features.Warehouses.DTOs
{
    public class UpdateWarehouseRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
    }
}
