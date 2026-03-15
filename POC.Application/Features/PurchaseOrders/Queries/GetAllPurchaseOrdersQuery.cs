using MediatR;
using POC.Application.Features.PurchaseOrders.DTOs;
using POC.Application.Features.PurchaseOrders.Helper;
using POC.Application.Features.PurchaseOrders.Interfaces;

namespace POC.Application.Features.PurchaseOrders.Queries
{
    public record GetAllPurchaseOrdersQuery() : IRequest<List<PurchaseOrderDto>>;

    public class GetAllPurchaseOrdersQueryHandler
        : IRequestHandler<GetAllPurchaseOrdersQuery, List<PurchaseOrderDto>>
    {
        private readonly IPurchaseOrderRepository _repository;

        public GetAllPurchaseOrdersQueryHandler(IPurchaseOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PurchaseOrderDto>> Handle(
            GetAllPurchaseOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var orders = await _repository.GetAllAsync(cancellationToken);

            return orders
                .Select(o => o.ToDto())
                .ToList();
        }
    }
}
