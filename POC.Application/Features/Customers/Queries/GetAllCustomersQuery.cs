using MediatR;
using POC.Application.Features.Customers.DTOs;
using POC.Application.Features.Customers.Helper;
using POC.Application.Features.Customers.Interfaces;

namespace POC.Application.Features.Customers.Queries
{
    public record GetAllCustomersQuery() : IRequest<List<CustomerDto>>;

    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerDto>>
    {
        private readonly ICustomerRepository _repository;

        public GetAllCustomersQueryHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _repository.GetAllAsync(cancellationToken);
            return customers.Select(x => x.ToDto()).ToList();
        }
    }
}
