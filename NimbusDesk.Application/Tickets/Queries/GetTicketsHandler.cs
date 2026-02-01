using FluentValidation;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Application.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public sealed class GetTicketsHandler
    {
        private readonly ITicketRepository _repository;
        private readonly IValidator<GetTicketsQuery> _validator;

        public GetTicketsHandler(ITicketRepository repository, IValidator<GetTicketsQuery> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<PagedResult<TicketSummaryDto>> Handle(
        GetTicketsQuery query,
        CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(query, cancellationToken);
            return await _repository.GetPagedAsync(query, cancellationToken);
        }



    }
}
