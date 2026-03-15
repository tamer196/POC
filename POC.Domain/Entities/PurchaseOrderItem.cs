namespace POC.Domain.Entities
{
    public class PurchaseOrderItem : BaseEntity
    {
        public Guid PurchaseOrderId { get; private set; }
        public PurchaseOrder PurchaseOrder { get; private set; } = default!;

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;

        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal LineTotal { get; private set; }

        private PurchaseOrderItem() { }

        public PurchaseOrderItem(Guid purchaseOrderId, Guid productId, int quantity, decimal unitPrice)
        {
            PurchaseOrderId = purchaseOrderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            LineTotal = quantity * unitPrice;
        }

        public void Update(int quantity, decimal unitPrice, Guid productId)
        {
            Quantity = quantity;
            UnitPrice = unitPrice;
            ProductId = productId;
            LineTotal = quantity * unitPrice;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
