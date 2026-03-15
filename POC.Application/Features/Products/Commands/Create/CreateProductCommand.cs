using MediatR;
using POC.Application.Features.Products.DTOs;
using POC.Application.Features.Products.Helper;
using POC.Application.Features.Products.Interfaces;
using POC.Domain.Entities;

namespace POC.Application.Features.Products.Commands.Create
{
    public record CreateProductCommand(CreateProductRequest Request) : IRequest<ProductDto>;

    public class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;

        public CreateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsBySkuAsync(request.Request.SKU, cancellationToken))
                throw new InvalidOperationException("SKU already exists.");

            var product = new Product(
                request.Request.SKU,
                request.Request.Name,
                request.Request.Description,
                request.Request.CostPrice,
                request.Request.SellingPrice,
                request.Request.QuantityInStock,
                request.Request.Status,
                request.Request.SupplierId,
                request.Request.WarehouseId);

            await _repository.AddAsync(product, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return product.ToDto();
        }
    }
}
