using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NimbusDesk.Application.Tickets;
using NimbusDesk.Domain.ValueObjects;

namespace NimbusDesk.API.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public sealed class TicketsController : ControllerBase
    {
        private readonly CreateTicketHandler _handler;

        public TicketsController(CreateTicketHandler handler)
        {
            _handler = handler;
        }
        public sealed record CreateTicketRequest
        (
            string Title,
            string Description,
            TicketPriority Priority
        );

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateTicketRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateTicketCommand(
                request.Title,
                request.Description,
                request.Priority);

            var ticketId = await _handler.Handle(command, cancellationToken);

            return CreatedAtAction(nameof(Create), new { id = ticketId }, null);
        }
    }
    
}
