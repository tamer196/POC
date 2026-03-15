using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POC.Application.Features.Warehouses.Commands.Create;
using POC.Application.Features.Warehouses.Commands.Delete;
using POC.Application.Features.Warehouses.Commands.Update;
using POC.Application.Features.Warehouses.DTOs;
using POC.Application.Features.Warehouses.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarehousesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WarehousesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<WarehouseDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllWarehousesQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<WarehouseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetWarehouseByIdQuery(id), cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseDto>> Create(
            [FromBody] CreateWarehouseRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateWarehouseCommand(request), cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<WarehouseDto>> Update(
            Guid id,
            [FromBody] UpdateWarehouseRequest request,
            CancellationToken cancellationToken)
        {
            request.Id = id;

            var result = await _mediator.Send(new UpdateWarehouseCommand(request), cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteWarehouseCommand(id), cancellationToken);

            return NoContent();
        }
    }
}
