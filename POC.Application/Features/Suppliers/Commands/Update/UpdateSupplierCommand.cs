using MediatR;
using POC.Application.Features.Suppliers.DTOs;
using POC.Application.Features.Suppliers.Helper;
using POC.Application.Features.Suppliers.Interfaces;

namespace POC.Application.Features.Suppliers.Commands.Update
{
    public record UpdateSupplierCommand(UpdateSupplierRequest Request) : IRequest<SupplierDto>;

    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, SupplierDto>
    {
        private readonly ISupplierRepository _repository;

        public UpdateSupplierCommandHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<SupplierDto> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _repository.GetByIdAsync(request.Request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Supplier not found.");

            supplier.Update(
                request.Request.Name,
                request.Request.Email,
                request.Request.Phone,
                request.Request.Address);

            _repository.Update(supplier);
            await _repository.SaveChangesAsync(cancellationToken);

            return supplier.ToDto();
        }
    }
}
