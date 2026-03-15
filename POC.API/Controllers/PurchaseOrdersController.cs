using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POC.Application.Features.PurchaseOrders.Commands.Create;
using POC.Application.Features.PurchaseOrders.Commands.Delete;
using POC.Application.Features.PurchaseOrders.Commands.Update;
using POC.Application.Features.PurchaseOrders.DTOs;
using POC.Application.Features.PurchaseOrders.Queries;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchaseOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PurchaseOrderDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllPurchaseOrdersQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PurchaseOrderDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPurchaseOrderByIdQuery(id), cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseOrderDto>> Create(
    CreatePurchaseOrderRequest request,
    CancellationToken cancellationToken)
        {
            // Get the userId from the token claims
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)
                ?? User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var result = await _mediator.Send(
                new CreatePurchaseOrderCommand(request, userId),
                cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PurchaseOrderDto>> Update(
    Guid id,
    UpdatePurchaseOrderRequest request,
    CancellationToken cancellationToken)
        {
            request.Id = id;

            var result = await _mediator.Send(
                new UpdatePurchaseOrderCommand(request),
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new DeletePurchaseOrderCommand(id),
                cancellationToken);

            return NoContent();
        }
    }
}
