using Microsoft.EntityFrameworkCore;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Application.Tickets.Queries;
using NimbusDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async Task<IReadOnlyList<TicketSummaryDto>> GetPagedAsync(
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

            return await tickets
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
        }

    }
}
