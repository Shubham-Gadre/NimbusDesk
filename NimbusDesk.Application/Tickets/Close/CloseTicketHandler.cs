using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Close
{
    public sealed class CloseTicketHandler
    {
        private readonly ITicketRepository _ticketRepository;

        public CloseTicketHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task Handle(
            CloseTicketCommand command,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository
                .GetByIdAsync(command.TicketId, cancellationToken);

            if (ticket is null)
                throw new DomainException("Ticket not found.");

            ticket.Close();

            await _ticketRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
