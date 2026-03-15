using FluentValidation;

namespace POC.Application.Features.PurchaseOrders.Commands.Create
{
    public class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrderCommand>
    {
        public CreatePurchaseOrderValidator()
        {
            RuleFor(x => x.Request.OrderNumber)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Request.SupplierId)
                .NotEmpty();

            RuleForEach(x => x.Request.Items)
                .ChildRules(items =>
                {
                    items.RuleFor(x => x.ProductId).NotEmpty();
                    items.RuleFor(x => x.Quantity).GreaterThan(0);
                    items.RuleFor(x => x.UnitPrice).GreaterThan(0);
                });
        }
    }
}
