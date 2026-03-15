using MediatR;
using POC.Application.Features.Suppliers.DTOs;
using POC.Application.Features.Suppliers.Helper;
using POC.Application.Features.Suppliers.Interfaces;
using POC.Domain.Entities;

namespace POC.Application.Features.Suppliers.Commands.Create
{
    public record CreateSupplierCommand(CreateSupplierRequest Request) : IRequest<SupplierDto>;

    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierDto>
    {
        private readonly ISupplierRepository _repository;

        public CreateSupplierCommandHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<SupplierDto> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByCodeAsync(request.Request.Code, cancellationToken))
                throw new InvalidOperationException("Supplier code already exists.");

            var supplier = new Supplier(
                request.Request.Code,
                request.Request.Name,
                request.Request.Email,
                request.Request.Phone,
                request.Request.Address);

            await _repository.AddAsync(supplier, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return supplier.ToDto();
        }
    }
}
