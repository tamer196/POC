using MediatR;
using POC.Application.Features.Suppliers.DTOs;
using POC.Application.Features.Suppliers.Helper;
using POC.Application.Features.Suppliers.Interfaces;

namespace POC.Application.Features.Suppliers.Queries
{
    public record GetSupplierByIdQuery(Guid Id) : IRequest<SupplierDto?>;

    public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, SupplierDto?>
    {
        private readonly ISupplierRepository _repository;

        public GetSupplierByIdQueryHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<SupplierDto?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _repository.GetByIdAsync(request.Id, cancellationToken);

            return supplier?.ToDto();
        }
    }

    
}
