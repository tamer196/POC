namespace POC.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Code { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Phone { get; private set; } = default!;
        public string Address { get; private set; } = default!;
        public bool IsActive { get; private set; } = true;

        private readonly List<SalesOrder> _salesOrders = new();
        public IReadOnlyCollection<SalesOrder> SalesOrders => _salesOrders;

        private Customer() { }

        public Customer(string code, string name, string email, string phone, string address)
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

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
