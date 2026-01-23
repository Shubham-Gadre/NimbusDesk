using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public sealed record GetTicketsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Status = null,
    string? Priority = null,
    string SortBy = "createdAt",
    string SortDirection = "desc"
);
}
