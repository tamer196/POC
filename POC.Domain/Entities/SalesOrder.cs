using POC.Domain.Enums;

namespace POC.Domain.Entities
{
    public class SalesOrder : BaseEntity
    {
        public string OrderNumber { get; private set; } = default!;
        public DateTime OrderDateUtc { get; private set; }
        public SalesOrderStatus Status { get; private set; }
        public decimal TotalAmount { get; private set; }

        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;

        public Guid CreatedByUserId { get; private set; }
        public User CreatedByUser { get; private set; } = default!;

        private readonly List<SalesOrderItem> _items = new();
        public IReadOnlyCollection<SalesOrderItem> Items => _items;

        private SalesOrder() { }

        public SalesOrder(string orderNumber, DateTime orderDateUtc, Guid customerId, Guid createdByUserId)
        {
            OrderNumber = orderNumber;
            OrderDateUtc = orderDateUtc;
            CustomerId = customerId;
            CreatedByUserId = createdByUserId;
            Status = SalesOrderStatus.Draft;
        }

        public void AddItem(Product product, int quantity, decimal unitPrice)
        {
            var item = new SalesOrderItem(Id, product.Id, quantity, unitPrice);
            _items.Add(item);
            RecalculateTotal();
        }

        public void ChangeStatus(SalesOrderStatus status)
        {
            Status = status;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void RecalculateTotal()
        {
            TotalAmount = _items.Sum(x => x.LineTotal);
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
