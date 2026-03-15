using MediatR;
using POC.Application.Features.Customers.DTOs;
using POC.Application.Features.Customers.Helper;
using POC.Application.Features.Customers.Interfaces;

namespace POC.Application.Features.Customers.Commands.Update
{
    public record UpdateCustomerCommand(UpdateCustomerRequest Request) : IRequest<CustomerDto>;

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _repository;

        public UpdateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.Request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Customer not found.");

            customer.Update(
                request.Request.Name,
                request.Request.Email,
                request.Request.Phone,
                request.Request.Address);

            _repository.Update(customer);
            await _repository.SaveChangesAsync(cancellationToken);

            return customer.ToDto();
        }
    }
}
