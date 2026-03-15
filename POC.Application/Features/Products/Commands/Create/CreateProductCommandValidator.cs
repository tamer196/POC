using FluentValidation;

namespace POC.Application.Features.Products.Commands.Create
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Request.SKU)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Request.Description)
                .NotEmpty();

            RuleFor(x => x.Request.CostPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Request.SellingPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Request.QuantityInStock)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Request.SupplierId)
                .NotEmpty();

            RuleFor(x => x.Request.WarehouseId)
                .NotEmpty();
        }
    }
}
