using MediatR;
using POC.Application.Features.Warehouses.DTOs;
using POC.Application.Features.Warehouses.Helper;
using POC.Application.Features.Warehouses.Interfaces;

namespace POC.Application.Features.Warehouses.Commands.Update
{
    public record UpdateWarehouseCommand(UpdateWarehouseRequest Request) : IRequest<WarehouseDto>;

    public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, WarehouseDto>
    {
        private readonly IWarehouseRepository _repository;

        public UpdateWarehouseCommandHandler(IWarehouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<WarehouseDto> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _repository.GetByIdAsync(request.Request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Warehouse not found.");

            warehouse.Update(
                request.Request.Name,
                request.Request.Location);

            _repository.Update(warehouse);

            await _repository.SaveChangesAsync(cancellationToken);

            return warehouse.ToDto();
        }
    }
}
