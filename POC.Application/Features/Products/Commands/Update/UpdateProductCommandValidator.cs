using FluentValidation;

namespace POC.Application.Features.Products.Commands.Update
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Request.Id)
                .NotEmpty();

            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Request.CostPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Request.SellingPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Request.SupplierId)
                .NotEmpty();

            RuleFor(x => x.Request.WarehouseId)
                .NotEmpty();
        }
    }
}
