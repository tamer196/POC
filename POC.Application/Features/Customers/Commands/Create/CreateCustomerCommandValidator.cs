using FluentValidation;

namespace POC.Application.Features.Customers.Commands.Create
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Request.Code)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(150);

            RuleFor(x => x.Request.Phone)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Request.Address)
                .NotEmpty()
                .MaximumLength(300);
        }
    }
}
