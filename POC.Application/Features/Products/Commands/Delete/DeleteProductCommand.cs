using MediatR;
using POC.Application.Features.Products.Interfaces;

namespace POC.Application.Features.Products.Commands.Delete
{
    public record DeleteProductCommand(Guid Id) : IRequest;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _repository;

        public DeleteProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Product not found.");

            _repository.Delete(product);

            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
