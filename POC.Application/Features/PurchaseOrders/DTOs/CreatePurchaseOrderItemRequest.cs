namespace POC.Application.Features.PurchaseOrders.DTOs
{
    public class CreatePurchaseOrderItemRequest
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }

}
