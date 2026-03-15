using MediatR;
using POC.Application.Features.Products.DTOs;
using POC.Application.Features.Products.Helper;
using POC.Application.Features.Products.Interfaces;

namespace POC.Application.Features.Products.Commands.Update
{
    public record UpdateProductCommand(UpdateProductRequest Request) : IRequest<ProductDto>;

    public class UpdateProductCommandHandler
        : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;

        public UpdateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Product not found.");

            product.Update(
                request.Request.Name,
                request.Request.Description,
                request.Request.CostPrice,
                request.Request.SellingPrice,
                request.Request.Status,
                request.Request.SupplierId,
                request.Request.WarehouseId);

            _repository.Update(product);
            await _repository.SaveChangesAsync(cancellationToken);

            return product.ToDto();
        }
    }
}
