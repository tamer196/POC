using FluentValidation;

namespace POC.Application.Features.Warehouses.Commands.Create
{
    public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
    {
        public CreateWarehouseCommandValidator()
        {
            RuleFor(x => x.Request.Code)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Request.Location)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
