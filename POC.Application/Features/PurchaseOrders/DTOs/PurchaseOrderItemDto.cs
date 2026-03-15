namespace POC.Application.Features.PurchaseOrders.DTOs
{
    public class PurchaseOrderItemDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal LineTotal { get; set; }
    }
}
