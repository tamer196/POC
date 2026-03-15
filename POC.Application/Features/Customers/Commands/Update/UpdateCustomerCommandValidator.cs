using FluentValidation;

namespace POC.Application.Features.Customers.Commands.Update
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Request.Id)
                .NotEmpty();

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
