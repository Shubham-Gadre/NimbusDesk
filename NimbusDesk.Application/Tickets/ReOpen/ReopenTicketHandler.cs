using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.ReOpen
{
    /// <summary>
    /// Handles the reopening of a closed ticket.
    /// Retrieves the ticket, reopens it, and persists the changes.
    /// </summary>
    public sealed class ReopenTicketHandler
    {
        private readonly ITicketRepository _ticketRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReopenTicketHandler"/> class.
        /// </summary>
        /// <param name="ticketRepository">The repository for ticket persistence operations.</param>
        public ReopenTicketHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        /// <summary>
        /// Handles the reopening of a closed ticket from the command.
        /// </summary>
        /// <param name="command">The command containing the ticket ID to reopen.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous reopen operation.</returns>
        /// <exception cref="DomainException">Thrown when the ticket is not found or cannot be reopened.</exception>
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
