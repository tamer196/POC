using MediatR;
using POC.Application.Features.PurchaseOrders.DTOs;
using POC.Application.Features.PurchaseOrders.Helper;
using POC.Application.Features.PurchaseOrders.Interfaces;

namespace POC.Application.Features.PurchaseOrders.Queries
{
    public record GetPurchaseOrderByIdQuery(Guid Id) : IRequest<PurchaseOrderDto?>;

    public class GetPurchaseOrderByIdQueryHandler
    : IRequestHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDto?>
    {
        private readonly IPurchaseOrderRepository _repository;

        public GetPurchaseOrderByIdQueryHandler(IPurchaseOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<PurchaseOrderDto?> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id, cancellationToken);

            return order?.ToDto();
        }
    }
}
