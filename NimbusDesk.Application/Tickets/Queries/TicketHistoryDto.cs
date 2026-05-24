using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    /// <summary>
    /// Represents a single historical change record for a ticket.
    /// Records what changed, from what value to what value, and when.
    /// </summary>
    /// <param name="ChangeType">The type of change that occurred (e.g., "StatusChanged", "TitleChanged").</param>
    /// <param name="FromValue">The value before the change.</param>
    /// <param name="ToValue">The value after the change.</param>
    /// <param name="ChangedAt">The date and time when the change was recorded.</param>
    public sealed record TicketHistoryDto(
        string ChangeType,
        string FromValue,
        string ToValue,
        DateTime ChangedAt
    );
}
