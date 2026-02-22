using NimbusDesk.Application.Abstraction.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public sealed class GetTicketDetailsHandler(ITicketRepository repository)
    {
        public async Task<TicketDetailsDto?> HandleAsync(Guid id, CancellationToken ct)
        {
            return await repository.GetDetailsAsync(id, ct);
        }
    }
}
