using NimbusDesk.Application.Abstraction.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    /// <summary>
    /// Handles the retrieval of the change history for a specific ticket.
    /// Returns all historical changes ordered by most recent first.
    /// </summary>
    public sealed class GetTicketHistoryHandler
    {
        private readonly ITicketRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTicketHistoryHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository for ticket history retrieval.</param>
        public GetTicketHistoryHandler(ITicketRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Handles retrieving the change history for a specific ticket.
        /// </summary>
        /// <param name="ticketId">The unique identifier of the ticket.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A read-only list of ticket history records ordered by most recent changes first.</returns>
        public async Task<IReadOnlyList<TicketHistoryDto>> Handle(
            Guid ticketId,
            CancellationToken cancellationToken)
        {
            return await _repository.GetHistoryAsync(ticketId, cancellationToken);
        }
    }
}



