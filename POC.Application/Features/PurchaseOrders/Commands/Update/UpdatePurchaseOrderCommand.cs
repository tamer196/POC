using MediatR;
using POC.Application.Features.PurchaseOrders.DTOs;
using POC.Application.Features.PurchaseOrders.Helper;
using POC.Application.Features.PurchaseOrders.Interfaces;
using POC.Domain.Entities;

namespace POC.Application.Features.PurchaseOrders.Commands.Update
{
    public record UpdatePurchaseOrderCommand(UpdatePurchaseOrderRequest Request): IRequest<PurchaseOrderDto>;

    public class UpdatePurchaseOrderCommandHandler
       : IRequestHandler<UpdatePurchaseOrderCommand, PurchaseOrderDto>
    {
        private readonly IPurchaseOrderRepository _repository;

        public UpdatePurchaseOrderCommandHandler(IPurchaseOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<PurchaseOrderDto> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Purchase order not found.");

            // Update status
            order.ChangeStatus(request.Request.Status);

            var existingItems = order.Items.ToDictionary(x => x.Id);

            foreach (var itemRequest in request.Request.Items)
            {
                if (existingItems.TryGetValue(itemRequest.Id, out var existingItem))
                {
                    existingItem.Update(
                        itemRequest.Quantity,
                        itemRequest.UnitPrice,
                        itemRequest.ProductId);
                }
                else
                {
                    order.AddItem(itemRequest.ProductId,itemRequest.Quantity,itemRequest.UnitPrice);
                }
            }

            // Remove deleted items
            var requestIds = request.Request.Items.Select(x => x.Id).ToHashSet();

            var itemsToRemove = order.Items
                .Where(x => !requestIds.Contains(x.Id))
                .ToList();

            foreach (var item in itemsToRemove)
            {
                order.RemoveItem(item.Id);
            }

            order.RecalculateTotal();

            _repository.Update(order);

            await _repository.SaveChangesAsync(cancellationToken);

            return order.ToDto();
        }
    }
}
