using NimbusDesk.Application.Abstraction.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public sealed class GetTicketHistoryHandler
    {
        private readonly ITicketRepository _repository;

        public GetTicketHistoryHandler(ITicketRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<TicketHistoryDto>> Handle(
            Guid ticketId,
            CancellationToken cancellationToken)
        {
            return await _repository.GetHistoryAsync(ticketId, cancellationToken);
        }
    }
}



