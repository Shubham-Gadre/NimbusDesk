using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public sealed class GetTicketsQueryValidator
    : AbstractValidator<GetTicketsQuery>
    {
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
