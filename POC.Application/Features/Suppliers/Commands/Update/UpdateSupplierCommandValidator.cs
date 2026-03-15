using FluentValidation;

namespace POC.Application.Features.Suppliers.Commands.Update
{
    public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
    {
        public UpdateSupplierCommandValidator()
        {
            RuleFor(x => x.Request.Id)
                .NotEmpty();

            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Request.Phone)
                .NotEmpty();

            RuleFor(x => x.Request.Address)
                .NotEmpty();
        }
    }
}
