using NimbusDesk.Application.Common;
using NimbusDesk.Application.Tickets.Queries;
using NimbusDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Abstraction.Persistence
{
    /// <summary>
    /// Defines the contract for ticket persistence operations.
    /// Provides methods for creating, retrieving, updating, and querying tickets.
    /// </summary>
    public interface ITicketRepository
    {
        /// <summary>
        /// Adds a new ticket to the repository.
        /// </summary>
        /// <param name="ticket">The ticket to add.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous add operation.</returns>
        Task AddAsync(Ticket ticket, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing ticket in the repository.
        /// </summary>
        /// <param name="ticket">The ticket with updated data.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous update operation.</returns>
        Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a ticket by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The ticket if found; otherwise, null.</returns>
        Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a paginated list of tickets with filtering and sorting options.
        /// </summary>
        /// <param name="query">The query containing pagination, filtering, and sorting parameters.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A paged result of ticket summaries matching the query criteria.</returns>
        Task<PagedResult<TicketSummaryDto>> GetPagedAsync(GetTicketsQuery query, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the change history for a specific ticket.
        /// </summary>
        /// <param name="ticketId">The unique identifier of the ticket.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A read-only list of ticket history records.</returns>
        Task<IReadOnlyList<TicketHistoryDto>> GetHistoryAsync(Guid ticketId, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves detailed information about a specific ticket, including its comments.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The ticket details if found; otherwise, null.</returns>
        Task<TicketDetailsDto?> GetDetailsAsync(Guid id, CancellationToken cancellationToken);



    }
}
