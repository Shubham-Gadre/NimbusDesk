using Microsoft.EntityFrameworkCore;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Application.Common;
using NimbusDesk.Application.Tickets.Queries;
using NimbusDesk.Domain.Entities;
using NimbusDesk.Domain.Exceptions;


namespace NimbusDesk.Infrastructure.Persistence
{
    public sealed class TicketRepository : ITicketRepository
    {
        private readonly NimbusDeskDbContext _context;

        public TicketRepository(NimbusDeskDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Ticket ticket,
            CancellationToken cancellationToken)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<Ticket?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
        {
            return await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<Ticket>> GetTickets(CancellationToken cancellationToken)
        {
            return await _context.Tickets
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

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



