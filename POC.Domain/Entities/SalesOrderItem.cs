namespace POC.Domain.Entities
{
    public class SalesOrderItem : BaseEntity
    {
        public Guid SalesOrderId { get; private set; }
        public SalesOrder SalesOrder { get; private set; } = default!;

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;

        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal LineTotal { get; private set; }

        private SalesOrderItem() { }

        public SalesOrderItem(Guid salesOrderId, Guid productId, int quantity, decimal unitPrice)
        {
            SalesOrderId = salesOrderId;
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
