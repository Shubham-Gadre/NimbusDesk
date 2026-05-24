using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Close
{
    /// <summary>
    /// Handles the closing of a ticket.
    /// Retrieves the ticket, closes it, and persists the changes.
    /// </summary>
    public sealed class CloseTicketHandler
    {
        private readonly ITicketRepository _ticketRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseTicketHandler"/> class.
        /// </summary>
        /// <param name="ticketRepository">The repository for ticket persistence operations.</param>
        public CloseTicketHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        /// <summary>
        /// Handles the closing of a ticket from the command.
        /// </summary>
        /// <param name="command">The command containing the ticket ID to close.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous close operation.</returns>
        /// <exception cref="DomainException">Thrown when the ticket is not found or cannot be closed.</exception>
        public async Task Handle(
            CloseTicketCommand command,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository
                .GetByIdAsync(command.TicketId, cancellationToken);

            if (ticket is null)
                throw new DomainException("Ticket not found.");

            ticket.Close();

            await _ticketRepository.UpdateAsync(ticket, cancellationToken);
        }
    }
}
