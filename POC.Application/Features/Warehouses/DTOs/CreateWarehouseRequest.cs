namespace POC.Application.Features.Warehouses.DTOs
{
    public class CreateWarehouseRequest
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
    }
}
