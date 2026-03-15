using POC.Domain.Enums;

namespace POC.Domain.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public string OrderNumber { get; private set; } = default!;
        public DateTime OrderDateUtc { get; private set; }
        public PurchaseOrderStatus Status { get; private set; }
        public decimal TotalAmount { get; private set; }

        public Guid SupplierId { get; private set; }
        public Supplier Supplier { get; private set; } = default!;

        public Guid CreatedByUserId { get; private set; }
        public User CreatedByUser { get; private set; } = default!;

        private readonly List<PurchaseOrderItem> _items = new();
        public IReadOnlyCollection<PurchaseOrderItem> Items => _items;

        private PurchaseOrder() { }

        public PurchaseOrder(string orderNumber, DateTime orderDateUtc, Guid supplierId, Guid createdByUserId)
        {
            OrderNumber = orderNumber;
            OrderDateUtc = orderDateUtc;
            SupplierId = supplierId;
            CreatedByUserId = createdByUserId;
            Status = PurchaseOrderStatus.Draft;
        }

        public void AddItem(Guid productId, int quantity, decimal unitPrice)
        {
            var item = new PurchaseOrderItem(Id, productId, quantity, unitPrice);
            _items.Add(item);
            RecalculateTotal();
        }

        public void ChangeStatus(PurchaseOrderStatus status)
        {
            Status = status;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void RecalculateTotal()
        {
            TotalAmount = _items.Sum(x => x.LineTotal);
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void RemoveItem(Guid itemId)
        {
            var item = _items.FirstOrDefault(x => x.Id == itemId);

            if (item != null)
                _items.Remove(item);

            RecalculateTotal();
        }
    }
}
