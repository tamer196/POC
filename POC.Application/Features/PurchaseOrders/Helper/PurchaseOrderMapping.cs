using POC.Application.Features.PurchaseOrders.DTOs;
using POC.Domain.Entities;

namespace POC.Application.Features.PurchaseOrders.Helper
{
    public static class PurchaseOrderMapping
    {
        public static PurchaseOrderDto ToDto(this PurchaseOrder order)
        {
            return new PurchaseOrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDateUtc = order.OrderDateUtc,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                SupplierId = order.SupplierId,
                CreatedByUserId = order.CreatedByUserId,
                Items = order.Items.Select(x => new PurchaseOrderItemDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    LineTotal = x.LineTotal
                }).ToList()
            };
        }
    }
}