using Microsoft.EntityFrameworkCore;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Application.Common;
using NimbusDesk.Application.Tickets.Queries;
using NimbusDesk.Domain.Entities;


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
            var tickets = _context.Tickets.AsQueryable();

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
                .OrderByDescending(t => t.CreatedAt)
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
                .Where(h => h.TicketId == ticketId)
                .OrderBy(h => h.ChangedAt)
                .Select(h => new TicketHistoryDto(
                    h.FromStatus,
                    h.ToStatus,
                    h.ChangedAt))
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            // If the entity instance is not currently tracked by this context, attach and mark modified.
            var entry = _context.ChangeTracker.Entries<Ticket>().FirstOrDefault(e => e.Entity == ticket);
            if (entry is null)
            {
                _context.Tickets.Attach(ticket);
                _context.Entry(ticket).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }


    }
}
