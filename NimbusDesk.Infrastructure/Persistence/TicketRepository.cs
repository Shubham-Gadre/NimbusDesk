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
    }
}
