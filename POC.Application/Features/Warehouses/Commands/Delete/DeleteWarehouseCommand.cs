using MediatR;
using POC.Application.Features.Warehouses.Interfaces;

namespace POC.Application.Features.Warehouses.Commands.Delete
{
    public record DeleteWarehouseCommand(Guid Id) : IRequest;

    public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand>
    {
        private readonly IWarehouseRepository _repository;

        public DeleteWarehouseCommandHandler(IWarehouseRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Warehouse not found.");

            _repository.Delete(warehouse);

            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
