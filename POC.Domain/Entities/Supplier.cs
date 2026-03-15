namespace POC.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public string Code { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Phone { get; private set; } = default!;
        public string Address { get; private set; } = default!;
        public bool IsActive { get; private set; } = true;

        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products;

        private readonly List<PurchaseOrder> _purchaseOrders = new();
        public IReadOnlyCollection<PurchaseOrder> PurchaseOrders => _purchaseOrders;

        private Supplier() { }

        public Supplier(string code, string name, string email, string phone, string address)
        {
            Code = code;
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
        }

        public void Update(string name, string email, string phone, string address)
        {
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
