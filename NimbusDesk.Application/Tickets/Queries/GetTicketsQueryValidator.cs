using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    /// <summary>
    /// Validator for the GetTicketsQuery.
    /// Validates pagination parameters, sort options, and sort direction according to business rules.
    /// </summary>
    public sealed class GetTicketsQueryValidator
    : AbstractValidator<GetTicketsQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTicketsQueryValidator"/> class.
        /// Configures validation rules for ticket queries.
        /// </summary>
        public GetTicketsQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);

            RuleFor(x => x.SortBy)
                .Must(TicketSortOptions.IsValid)
                .WithMessage("Invalid sort field.");

            RuleFor(x => x.SortDirection)
                .Must(v => v is "asc" or "desc")
                .WithMessage("Sort direction must be 'asc' or 'desc'.");
        }
    }
}
