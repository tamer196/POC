namespace POC.Domain.Enums
{
    public enum UserRole
    {
        Admin = 1,
        Manager = 2,
        User = 3
    }

    public enum ProductStatus
    {
        Active = 1,
        Discontinued = 2,
        OutOfStock = 3
    }

    public enum PurchaseOrderStatus
    {
        Draft = 1,
        Submitted = 2,
        Received = 3,
        Cancelled = 4
    }

    public enum SalesOrderStatus
    {
        Draft = 1,
        Confirmed = 2,
        Shipped = 3,
        Cancelled = 4
    }
}
