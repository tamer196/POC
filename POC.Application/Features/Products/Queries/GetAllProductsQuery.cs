using MediatR;
using POC.Application.Features.Products.DTOs;
using POC.Application.Features.Products.Helper;
using POC.Application.Features.Products.Interfaces;

namespace POC.Application.Features.Products.Queries
{
    public record GetAllProductsQuery() : IRequest<List<ProductDto>>;

    public class GetAllProductsQueryHandler
        : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllAsync(cancellationToken);

            return products.Select(x => x.ToDto()).ToList();
        }
    }
}
