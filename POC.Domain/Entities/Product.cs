using POC.Domain.Enums;

namespace POC.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string SKU { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public decimal CostPrice { get; private set; }
        public decimal SellingPrice { get; private set; }
        public int QuantityInStock { get; private set; }
        public ProductStatus Status { get; private set; }

        public Guid SupplierId { get; private set; }
        public Supplier Supplier { get; private set; } = default!;

        public Guid WarehouseId { get; private set; }
        public Warehouse Warehouse { get; private set; } = default!;

        private readonly List<PurchaseOrderItem> _purchaseOrderItems = new();
        public IReadOnlyCollection<PurchaseOrderItem> PurchaseOrderItems => _purchaseOrderItems;

        private readonly List<SalesOrderItem> _salesOrderItems = new();
        public IReadOnlyCollection<SalesOrderItem> SalesOrderItems => _salesOrderItems;

        private Product() { }

        public Product(
            string sku,
            string name,
            string description,
            decimal costPrice,
            decimal sellingPrice,
            int quantityInStock,
            ProductStatus status,
            Guid supplierId,
            Guid warehouseId)
        {
            SKU = sku;
            Name = name;
            Description = description;
            CostPrice = costPrice;
            SellingPrice = sellingPrice;
            QuantityInStock = quantityInStock;
            Status = status;
            SupplierId = supplierId;
            WarehouseId = warehouseId;
        }

        public void Update(
            string name,
            string description,
            decimal costPrice,
            decimal sellingPrice,
            ProductStatus status,
            Guid supplierId,
            Guid warehouseId)
        {
            Name = name;
            Description = description;
            CostPrice = costPrice;
            SellingPrice = sellingPrice;
            Status = status;
            SupplierId = supplierId;
            WarehouseId = warehouseId;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void IncreaseStock(int quantity)
        {
            QuantityInStock += quantity;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void DecreaseStock(int quantity)
        {
            QuantityInStock -= quantity;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
