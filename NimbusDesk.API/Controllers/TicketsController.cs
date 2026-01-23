using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Application.Common;
using NimbusDesk.Application.Tickets.Close;
using NimbusDesk.Application.Tickets.Create;
using NimbusDesk.Application.Tickets.Queries;
using NimbusDesk.Domain.ValueObjects;
using NimbusDesk.Infrastructure.Persistence;

namespace NimbusDesk.API.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public sealed class TicketsController : ControllerBase
    {
        private readonly CreateTicketHandler _handler;
        private readonly CloseTicketHandler _closeTicketHandler;
        private readonly ITicketRepository _repository;
        private readonly GetTicketsHandler _getTicketsHandler;

        public TicketsController(CreateTicketHandler handler, CloseTicketHandler closeHandler, ITicketRepository repository, GetTicketsHandler getTicketsHandler)
        {
            _handler = handler;
            _closeTicketHandler = closeHandler;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _getTicketsHandler = getTicketsHandler ?? throw new ArgumentNullException(nameof(getTicketsHandler));
        }
        public sealed record CreateTicketRequest
        (
            string Title,
            string Description,
            string Priority
        );

        public sealed record TicketDto(
            Guid Id,
            string Title,
            string Description,
            TicketStatus Status,
            string Priority,
            DateTime CreatedAt,
            DateTime? ClosedAt
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

            return CreatedAtAction(nameof(GetById), new { id = ticketId }, null);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var ticket = await _repository.GetByIdAsync(id, cancellationToken);
            if (ticket is null) return NotFound();

            var dto = new TicketDto(
                ticket.Id,
                ticket.Title,
                ticket.Description,
                ticket.Status,
                ticket.Priority.Value,
                ticket.CreatedAt,
                ticket.ClosedAt);

            return Ok(dto);

        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TicketSummaryDto>>> Get(
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 20,
                [FromQuery] string? status = null,
                [FromQuery] string? priority = null,
                CancellationToken cancellationToken = default)
        {
            var query = new GetTicketsQuery(
                page,
                pageSize,
                status,
                priority);

            var result = await _getTicketsHandler
                .Handle(query, cancellationToken);

            return Ok(result);
        }



        [HttpPost("{id:guid}/close")]
        public async Task<IActionResult> Close(
                            Guid id,
                            CancellationToken cancellationToken)
        {   
            var command = new CloseTicketCommand(id);

            await _closeTicketHandler.Handle(command, cancellationToken);

            return NoContent();
        }



    }

}
