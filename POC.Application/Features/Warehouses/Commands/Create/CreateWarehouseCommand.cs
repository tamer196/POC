using MediatR;
using POC.Application.Features.Warehouses.DTOs;
using POC.Application.Features.Warehouses.Helper;
using POC.Application.Features.Warehouses.Interfaces;
using POC.Domain.Entities;

namespace POC.Application.Features.Warehouses.Commands.Create
{
    public record CreateWarehouseCommand(CreateWarehouseRequest Request) : IRequest<WarehouseDto>;
    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, WarehouseDto>
    {
        private readonly IWarehouseRepository _repository;

        public CreateWarehouseCommandHandler(IWarehouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<WarehouseDto> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByCodeAsync(request.Request.Code, cancellationToken))
                throw new InvalidOperationException("Warehouse code already exists.");

            var warehouse = new Warehouse(
                request.Request.Code,
                request.Request.Name,
                request.Request.Location);

            await _repository.AddAsync(warehouse, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return warehouse.ToDto();
        }
    }
}
