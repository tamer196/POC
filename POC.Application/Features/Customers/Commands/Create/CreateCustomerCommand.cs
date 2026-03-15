using MediatR;
using POC.Application.Features.Customers.DTOs;
using POC.Application.Features.Customers.Helper;
using POC.Application.Features.Customers.Interfaces;
using POC.Domain.Entities;

namespace POC.Application.Features.Customers.Commands.Create
{
    public record CreateCustomerCommand(CreateCustomerRequest Request) : IRequest<CustomerDto>;

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _repository;

        public CreateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByCodeAsync(request.Request.Code))
                throw new InvalidOperationException("Customer code already exists.");

            var customer = new Customer(
                request.Request.Code,
                request.Request.Name,
                request.Request.Email,
                request.Request.Phone,
                request.Request.Address);

            await _repository.AddAsync(customer, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return customer.ToDto();
        }
    }

}
