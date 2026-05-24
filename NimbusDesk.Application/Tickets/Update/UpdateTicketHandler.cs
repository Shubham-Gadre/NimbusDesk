using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Exceptions;
using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Update
{
    /// <summary>
    /// Handles the updating of ticket details.
    /// Retrieves the ticket, updates its properties, and persists the changes.
    /// </summary>
    public sealed class UpdateTicketHandler
    {
        private readonly ITicketRepository _ticketRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTicketHandler"/> class.
        /// </summary>
        /// <param name="ticketRepository">The repository for ticket persistence operations.</param>
        public UpdateTicketHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        /// <summary>
        /// Handles the updating of ticket details from the command.
        /// </summary>
        /// <param name="command">The command containing the ticket ID and new details.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous update operation.</returns>
        /// <exception cref="DomainException">Thrown when the ticket is not found or cannot be updated.</exception>
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
