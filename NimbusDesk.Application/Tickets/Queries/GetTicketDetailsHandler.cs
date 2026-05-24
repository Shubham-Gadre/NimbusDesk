using NimbusDesk.Application.Abstraction.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    /// <summary>
    /// Handles the retrieval of detailed information about a specific ticket, including its comments.
    /// </summary>
    /// <param name="repository">The repository for ticket data retrieval.</param>
    public sealed class GetTicketDetailsHandler(ITicketRepository repository)
    {
        /// <summary>
        /// Handles retrieving detailed information about a specific ticket.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket.</param>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The ticket details if found; otherwise, null.</returns>
        public async Task<TicketDetailsDto?> HandleAsync(Guid id, CancellationToken ct)
        {
            return await repository.GetDetailsAsync(id, ct);
        }
    }
}
