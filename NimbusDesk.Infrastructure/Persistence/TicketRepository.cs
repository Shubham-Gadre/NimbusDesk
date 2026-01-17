using Microsoft.EntityFrameworkCore;
using NimbusDesk.Application.Abstraction.Persistence;
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

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
