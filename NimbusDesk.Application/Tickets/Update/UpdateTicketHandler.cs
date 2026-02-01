using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Exceptions;
using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Update
{
    public sealed class UpdateTicketHandler
    {
        private readonly ITicketRepository _ticketRepository;

        public UpdateTicketHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task Handle(
            UpdateTicketCommand command,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository
                .GetByIdAsync(command.TicketId, cancellationToken);

            if (ticket is null)
                throw new DomainException("Ticket not found.");

            var priority = TicketPriority.FromValue(command.Priority);

            ticket.UpdateDetails(
                command.Title,
                command.Description,
                priority);

            await _ticketRepository.UpdateAsync(ticket, cancellationToken);
        }
    }

}
