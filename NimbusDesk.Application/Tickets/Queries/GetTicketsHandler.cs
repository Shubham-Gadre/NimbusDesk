using FluentValidation;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Application.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    /// <summary>
    /// Handles the retrieval of a paginated list of tickets with filtering and sorting.
    /// Validates the query parameters and returns filtered, sorted, and paginated results.
    /// </summary>
    public sealed class GetTicketsHandler
    {
        private readonly ITicketRepository _repository;
        private readonly IValidator<GetTicketsQuery> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTicketsHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository for ticket data retrieval.</param>
        /// <param name="validator">The validator for GetTicketsQuery validation.</param>
        public GetTicketsHandler(ITicketRepository repository, IValidator<GetTicketsQuery> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        /// <summary>
        /// Handles the retrieval of tickets based on the provided query.
        /// </summary>
        /// <param name="query">The query containing filtering, sorting, and pagination parameters.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A paged result of ticket summaries matching the query criteria.</returns>
        public async Task<PagedResult<TicketSummaryDto>> Handle(
        GetTicketsQuery query,
        CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(query, cancellationToken);
            return await _repository.GetPagedAsync(query, cancellationToken);
        }



    }
}
