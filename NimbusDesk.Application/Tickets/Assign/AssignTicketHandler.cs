using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Assign
{
    /// <summary>
    /// Handles the assignment of a ticket to a user.
    /// Retrieves the ticket and updates its assigned user.
    /// </summary>
    /// <param name="repository">The repository for ticket persistence operations.</param>
    public sealed class AssignTicketHandler(ITicketRepository repository)
    {
        /// <summary>
        /// Handles the assignment of a ticket to a user.
        /// </summary>
        /// <param name="command">The command containing the ticket ID and user ID for assignment.</param>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous assignment operation.</returns>
        /// <exception cref="DomainException">Thrown when the ticket is not found.</exception>
        public async Task HandleAsync(AssignTicketCommand command, CancellationToken ct)
        {
            var ticket = await repository.GetByIdAsync(command.TicketId, ct)
                ?? throw new DomainException("Ticket not found.");

            ticket.Assign(command.UserId);

            await repository.UpdateAsync(ticket, ct);
        }
    }
}
