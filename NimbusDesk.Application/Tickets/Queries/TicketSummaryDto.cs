using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public sealed record TicketSummaryDto(
        Guid Id,
        string Title,
        string Status,
        string Priority,
        DateTime CreatedAt
    );
    
}
