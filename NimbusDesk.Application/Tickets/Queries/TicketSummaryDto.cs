using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    /// <summary>
    /// Represents a summary of a ticket for list display purposes.
    /// Contains essential ticket information for overview presentations.
    /// </summary>
    /// <param name="Id">The unique identifier of the ticket.</param>
    /// <param name="Title">The title of the ticket.</param>
    /// <param name="Status">The current status of the ticket.</param>
    /// <param name="Priority">The priority level of the ticket.</param>
    /// <param name="CreatedAt">The date and time when the ticket was created.</param>
    public sealed record TicketSummaryDto(
        Guid Id,
        string Title,
        string Status,
        string Priority,
        DateTime CreatedAt
    );
    
}
