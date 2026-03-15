using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POC.Application.Features.Suppliers.Commands.Create;
using POC.Application.Features.Suppliers.Commands.Delete;
using POC.Application.Features.Suppliers.Commands.Update;
using POC.Application.Features.Suppliers.DTOs;
using POC.Application.Features.Suppliers.Queries;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SuppliersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<SupplierDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllSuppliersQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDto>> Create(
            [FromBody] CreateSupplierRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateSupplierCommand(request), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierDto>> Update(
            Guid id,
            [FromBody] UpdateSupplierRequest request,
            CancellationToken cancellationToken)
        {
            request.Id = id;

            var result = await _mediator.Send(new UpdateSupplierCommand(request), cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteSupplierCommand(id), cancellationToken);

            return NoContent();
        }
    }
}
