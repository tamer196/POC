using FluentValidation;

namespace POC.Application.Features.Suppliers.Commands.Create
{
    public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierCommandValidator()
        {
            RuleFor(x => x.Request.Code)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Request.Phone)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Request.Address)
                .NotEmpty()
                .MaximumLength(300);
        }
    }
}
