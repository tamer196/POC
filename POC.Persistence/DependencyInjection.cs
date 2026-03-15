using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POC.Application.Features.Customers.Interfaces;
using POC.Application.Features.Products.Interfaces;
using POC.Application.Features.PurchaseOrders.Interfaces;
using POC.Application.Features.Suppliers.Interfaces;
using POC.Application.Features.Warehouses.Interfaces;
using POC.Application.Users.Interfaces;
using POC.Persistence.Repositories;

namespace POC.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSql")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();

            return services;
        }
    }
}
