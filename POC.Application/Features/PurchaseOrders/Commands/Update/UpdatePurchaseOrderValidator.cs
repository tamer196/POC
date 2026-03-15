using FluentValidation;

namespace POC.Application.Features.PurchaseOrders.Commands.Update
{
    public class UpdatePurchaseOrderValidator : AbstractValidator<UpdatePurchaseOrderCommand>
    {
        public UpdatePurchaseOrderValidator()
        {
            RuleFor(x => x.Request.Id)
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
