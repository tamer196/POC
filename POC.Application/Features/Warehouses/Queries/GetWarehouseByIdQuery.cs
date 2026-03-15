using MediatR;
using POC.Application.Features.Warehouses.DTOs;
using POC.Application.Features.Warehouses.Helper;
using POC.Application.Features.Warehouses.Interfaces;

namespace POC.Application.Features.Warehouses.Queries
{
    public record GetWarehouseByIdQuery(Guid Id) : IRequest<WarehouseDto?>;

    public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseDto?>
    {
        private readonly IWarehouseRepository _repository;

        public GetWarehouseByIdQueryHandler(IWarehouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<WarehouseDto?> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _repository.GetByIdAsync(request.Id, cancellationToken);

            return warehouse?.ToDto();
        }
    }
}
