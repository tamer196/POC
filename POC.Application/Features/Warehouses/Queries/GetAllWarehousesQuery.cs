using MediatR;
using POC.Application.Features.Warehouses.DTOs;
using POC.Application.Features.Warehouses.Helper;
using POC.Application.Features.Warehouses.Interfaces;

namespace POC.Application.Features.Warehouses.Queries
{
    public record GetAllWarehousesQuery() : IRequest<List<WarehouseDto>>;

    public class GetAllWarehousesQueryHandler : IRequestHandler<GetAllWarehousesQuery, List<WarehouseDto>>
    {
        private readonly IWarehouseRepository _repository;

        public GetAllWarehousesQueryHandler(IWarehouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<WarehouseDto>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
        {
            var warehouses = await _repository.GetAllAsync(cancellationToken);

            return warehouses.Select(x => x.ToDto()).ToList();
        }
    }
}
