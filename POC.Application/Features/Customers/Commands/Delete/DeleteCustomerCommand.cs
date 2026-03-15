using MediatR;
using POC.Application.Features.Customers.Interfaces;

namespace POC.Application.Features.Customers.Commands.Delete
{
    public record DeleteCustomerCommand(Guid Id) : IRequest;

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly ICustomerRepository _repository;

        public DeleteCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Customer not found.");

            _repository.Delete(customer);
            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
