using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.ReOpen
{
    public sealed class ReopenTicketHandler
    {
        private readonly ITicketRepository _ticketRepository;

        public ReopenTicketHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task Handle(
            ReopenTicketCommand command,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository
                .GetByIdAsync(command.TicketId, cancellationToken);

            if (ticket is null)
                throw new DomainException("Ticket not found.");

            ticket.Reopen();

            await _ticketRepository.UpdateAsync(ticket, cancellationToken);
        }
    }
}
