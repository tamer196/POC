using FluentValidation;

namespace POC.Application.Features.Warehouses.Commands.Update
{
    public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
    {
        public UpdateWarehouseCommandValidator()
        {
            RuleFor(x => x.Request.Id)
                .NotEmpty();

            RuleFor(x => x.Request.Name)
                .NotEmpty();

            RuleFor(x => x.Request.Location)
                .NotEmpty();
        }
    }
}
