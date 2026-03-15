using MediatR;
using POC.Application.Features.Suppliers.Interfaces;

namespace POC.Application.Features.Suppliers.Commands.Delete
{
    public record DeleteSupplierCommand(Guid Id) : IRequest;

    public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand>
    {
        private readonly ISupplierRepository _repository;

        public DeleteSupplierCommandHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Supplier not found.");

            _repository.Delete(supplier);

            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
