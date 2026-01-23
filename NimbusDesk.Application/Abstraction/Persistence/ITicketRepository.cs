using NimbusDesk.Application.Common;
using NimbusDesk.Application.Tickets.Queries;
using NimbusDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Abstraction.Persistence
{
    public interface ITicketRepository
    {
        Task AddAsync(Ticket ticket, CancellationToken cancellationToken);
        Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
       
        //Task<IReadOnlyList<TicketSummaryDto>> GetPagedAsync(GetTicketsQuery query,CancellationToken cancellationToken);
        Task<PagedResult<TicketSummaryDto>> GetPagedAsync(
    GetTicketsQuery query,
    CancellationToken cancellationToken);

    }
}
