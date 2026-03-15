using MediatR;
using POC.Application.Features.PurchaseOrders.DTOs;
using POC.Application.Features.PurchaseOrders.Helper;
using POC.Application.Features.PurchaseOrders.Interfaces;
using POC.Domain.Entities;

namespace POC.Application.Features.PurchaseOrders.Commands.Create
{
    public record CreatePurchaseOrderCommand(CreatePurchaseOrderRequest Request,Guid userID)
    : IRequest<PurchaseOrderDto>;

    public class CreatePurchaseOrderCommandHandler
    : IRequestHandler<CreatePurchaseOrderCommand, PurchaseOrderDto>
    {
        private readonly IPurchaseOrderRepository _repository;

        public CreatePurchaseOrderCommandHandler(IPurchaseOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<PurchaseOrderDto> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByOrderNumberAsync(request.Request.OrderNumber, cancellationToken))
                throw new InvalidOperationException("Order number already exists.");

            
            var order = new PurchaseOrder(
                request.Request.OrderNumber,
                request.Request.OrderDateUtc,
                request.Request.SupplierId,
                request.userID);

            foreach (var item in request.Request.Items)
            {
                order.AddItem(item.ProductId,item.Quantity,item.UnitPrice);
            }

            await _repository.AddAsync(order, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return order.ToDto();
        }
    }
}
