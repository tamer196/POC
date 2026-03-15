using MediatR;
using POC.Application.Features.Products.DTOs;
using POC.Application.Features.Products.Helper;
using POC.Application.Features.Products.Interfaces;

namespace POC.Application.Features.Products.Queries
{
    public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;

    public class GetProductByIdQueryHandler
        : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

            return product?.ToDto();
        }
    }
}
