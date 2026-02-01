using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public sealed record TicketHistoryDto(
    string FromStatus,
    string ToStatus,
    DateTime ChangedAt
);
}
