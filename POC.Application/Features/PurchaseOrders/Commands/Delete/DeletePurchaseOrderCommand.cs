using MediatR;
using POC.Application.Features.PurchaseOrders.Interfaces;

namespace POC.Application.Features.PurchaseOrders.Commands.Delete
{
    public record DeletePurchaseOrderCommand(Guid Id) : IRequest;

    public class DeletePurchaseOrderCommandHandler : IRequestHandler<DeletePurchaseOrderCommand>
    {
        private readonly IPurchaseOrderRepository _repository;

        public DeletePurchaseOrderCommandHandler(IPurchaseOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Purchase order not found.");

            _repository.Delete(order);

            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
