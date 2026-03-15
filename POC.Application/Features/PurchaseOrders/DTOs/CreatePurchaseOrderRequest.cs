namespace POC.Application.Features.PurchaseOrders.DTOs
{
    public class CreatePurchaseOrderRequest
    {
        public string OrderNumber { get; set; } = default!;

        public DateTime OrderDateUtc { get; set; }

        public Guid SupplierId { get; set; }

        public List<CreatePurchaseOrderItemRequest> Items { get; set; } = new();
    }
}
