using POC.Domain.Enums;

namespace POC.Application.Features.PurchaseOrders.DTOs
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }

        public string OrderNumber { get; set; } = default!;

        public DateTime OrderDateUtc { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }

        public Guid SupplierId { get; set; }

        public Guid CreatedByUserId { get; set; }

        public List<PurchaseOrderItemDto> Items { get; set; } = new();
    }
}
