using MediatR;
using POC.Application.Features.Suppliers.DTOs;
using POC.Application.Features.Suppliers.Helper;
using POC.Application.Features.Suppliers.Interfaces;

namespace POC.Application.Features.Suppliers.Queries
{
    public record GetAllSuppliersQuery() : IRequest<List<SupplierDto>>;

    public class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, List<SupplierDto>>
    {
        private readonly ISupplierRepository _repository;

        public GetAllSuppliersQueryHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _repository.GetAllAsync(cancellationToken);

            return suppliers.Select(x => x.ToDto()).ToList();
        }
    }
}
