using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Comment
{
    /// <summary>
    /// Handles the addition of a comment to a ticket.
    /// Retrieves the ticket and adds a new comment to it.
    /// </summary>
    /// <param name="repository">The repository for ticket persistence operations.</param>
    public sealed class AddCommentHandler(ITicketRepository repository)
    {
        /// <summary>
        /// Handles adding a comment to a ticket from the command.
        /// </summary>
        /// <param name="command">The command containing the ticket ID, user ID, and comment content.</param>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous comment addition operation.</returns>
        /// <exception cref="DomainException">Thrown when the ticket is not found or is closed.</exception>
        public async Task HandleAsync(AddCommentCommand command, CancellationToken ct)
        {
            var ticket = await repository.GetByIdAsync(command.TicketId, ct)
                ?? throw new DomainException("Ticket not found.");

            ticket.AddComment(command.UserId, command.Content);

            await repository.UpdateAsync(ticket, ct);
        }
    }
}
