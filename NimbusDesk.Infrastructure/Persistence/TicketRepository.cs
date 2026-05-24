using Microsoft.EntityFrameworkCore;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Application.Common;
using NimbusDesk.Application.Tickets.Queries;
using NimbusDesk.Domain.Entities;
using NimbusDesk.Domain.Exceptions;


namespace NimbusDesk.Infrastructure.Persistence
{
    /// <summary>
    /// Repository implementation for ticket persistence operations.
    /// Provides access to ticket data including creation, retrieval, updates, and related data queries.
    /// </summary>
    public sealed class TicketRepository : ITicketRepository
    {
        private readonly NimbusDeskDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing ticket data.</param>
        public TicketRepository(NimbusDeskDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new ticket to the database and persists the changes.
        /// </summary>
        /// <param name="ticket">The ticket to add to the database.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous add operation.</returns>
        public async Task AddAsync(
            Ticket ticket,
            CancellationToken cancellationToken)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a ticket by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The ticket if found; otherwise, null.</returns>
        public async Task<Ticket?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
        {
            return await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        /// <summary>
        /// Retrieves all tickets from the database.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A list of all tickets.</returns>
        public async Task<List<Ticket>> GetTickets(CancellationToken cancellationToken)
        {
            return await _context.Tickets
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Persists all pending changes in the database context to the database.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous save operation.</returns>
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a paged result of tickets based on the provided query filters and sorting options.
        /// Supports filtering by status and priority, and sorting by creation date, priority, or status.
        /// </summary>
        /// <param name="query">The query parameters including filters, sorting, and pagination options.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A paged result containing ticket summaries matching the query criteria.</returns>
        public async Task<PagedResult<TicketSummaryDto>> GetPagedAsync(
    GetTicketsQuery query,
    CancellationToken cancellationToken)
        {
            var tickets = _context.Tickets
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                tickets = tickets.Where(t => t.Status.Value == query.Status);
            }

            if (!string.IsNullOrWhiteSpace(query.Priority))
            {
                tickets = tickets.Where(t => t.Priority.Value == query.Priority);
            }

            var totalCount = await tickets.CountAsync(cancellationToken);

            tickets = query.SortBy switch
            {
                TicketSortOptions.Priority => query.SortDirection == "asc"
                    ? tickets.OrderBy(t => t.Priority.Value)
                    : tickets.OrderByDescending(t => t.Priority.Value),

                TicketSortOptions.Status => query.SortDirection == "asc"
                    ? tickets.OrderBy(t => t.Status.Value)
                    : tickets.OrderByDescending(t => t.Status.Value),

                _ => query.SortDirection == "asc"
                    ? tickets.OrderBy(t => t.CreatedAt)
                    : tickets.OrderByDescending(t => t.CreatedAt)
            };

            var items = await tickets
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(t => new TicketSummaryDto(
                    t.Id,
                    t.Title,
                    t.Status.Value,
                    t.Priority.Value,
                    t.CreatedAt))
                .ToListAsync(cancellationToken);

            return new PagedResult<TicketSummaryDto>(
                items,
                query.Page,
                query.PageSize,
                totalCount);
        }


        /// <summary>
        /// Retrieves the change history for a specific ticket.
        /// </summary>
        /// <param name="ticketId">The unique identifier of the ticket.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A read-only list of ticket history records ordered by most recent changes first.</returns>
        public async Task<IReadOnlyList<TicketHistoryDto>> GetHistoryAsync(Guid ticketId, CancellationToken cancellationToken)
        {
            return await _context.Set<TicketHistory>()
                .AsNoTracking()
                .Where(h => h.TicketId == ticketId)
                .OrderByDescending(h => h.ChangedAt) // Newest changes first
                .Select(h => new TicketHistoryDto(
                    h.ChangeType,
                    h.FromValue,
                    h.ToValue,
                    h.ChangedAt))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Updates an existing ticket in the database, including any added comments.
        /// Handles optimistic concurrency conflicts with appropriate error handling.
        /// </summary>
        /// <param name="ticket">The ticket to update.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous update operation.</returns>
        /// <exception cref="DomainException">Thrown when a concurrency conflict occurs during save.</exception>
        public async Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            try
            {
                // Explicitly update only the ticket (with RowVersion for concurrency)
                // Child entities (Comments, History) are already tracked or will be added by EF
                foreach (var comment in ticket.Comments)
                {
                    var commentEntry = _context.Entry(comment);
                    if (commentEntry.State == EntityState.Detached)
                    {
                        _context.Entry(comment).State = EntityState.Added;
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Simple, clean error handling for your career flagship
                throw new DomainException("The ticket was modified by another user. Please refresh.");
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific ticket, including its comments.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The ticket details if found; otherwise, null.</returns>
        public async Task<TicketDetailsDto?> GetDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TicketDetailsDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Status.Value,
                    t.Priority.Value,
                    t.AssignedToUserId,
                    t.CreatedAt,
                    t.ClosedAt,
                    t.Comments.Select(c => new CommentDto(
                        c.Id,
                        c.UserId,
                        c.Content,
                        c.CreatedAt)).ToList()
                ))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}



