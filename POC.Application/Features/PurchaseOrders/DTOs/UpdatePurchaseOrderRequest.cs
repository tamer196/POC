using POC.Domain.Enums;

namespace POC.Application.Features.PurchaseOrders.DTOs
{
    public class UpdatePurchaseOrderRequest
    {
        public Guid Id { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        public List<UpdatePurchaseOrderItemRequest> Items { get; set; } = new();
    }

}
