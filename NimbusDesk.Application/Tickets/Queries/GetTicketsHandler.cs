using NimbusDesk.Application.Abstraction.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public sealed class GetTicketsHandler
    {
        private readonly ITicketRepository _repository;

        public GetTicketsHandler(ITicketRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<TicketSummaryDto>> Handle(
            GetTicketsQuery query,
            CancellationToken cancellationToken)
        {
            return await _repository.GetPagedAsync(query, cancellationToken);
        }
    }
}
