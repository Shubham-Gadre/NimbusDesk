using NimbusDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Abstraction.Persistence
{
    public interface ITicketRepository
    {
        Task AddAsync(Ticket ticket, CancellationToken cancellationToken);
    }
}
