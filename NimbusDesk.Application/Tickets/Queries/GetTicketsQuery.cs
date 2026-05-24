using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    /// <summary>
    /// Represents a query to retrieve a paginated list of tickets with filtering and sorting options.
    /// </summary>
    /// <param name="Page">The page number for pagination (default: 1).</param>
    /// <param name="PageSize">The number of tickets per page (default: 20).</param>
    /// <param name="Status">Optional filter by ticket status (e.g., "Open", "Closed").</param>
    /// <param name="Priority">Optional filter by ticket priority (e.g., "Low", "Medium", "High").</param>
    /// <param name="SortBy">The field to sort by (default: "createdAt"). Options: "createdAt", "priority", "status".</param>
    /// <param name="SortDirection">The sort direction (default: "desc"). Options: "asc" or "desc".</param>
    public sealed record GetTicketsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Status = null,
    string? Priority = null,
    string SortBy = "createdAt",
    string SortDirection = "desc"
);
}
