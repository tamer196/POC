namespace POC.Domain.Entities
{
    public class Warehouse : BaseEntity
    {
        public string Code { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public string Location { get; private set; } = default!;
        public bool IsActive { get; private set; } = true;

        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products;

        private Warehouse() { }

        public Warehouse(string code, string name, string location)
        {
            Code = code;
            Name = name;
            Location = location;
        }

        public void Update(string name, string location)
        {
            Name = name;
            Location = location;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
