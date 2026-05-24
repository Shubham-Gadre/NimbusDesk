using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.ReOpen
{
    /// <summary>
    /// Represents a command to reopen a closed ticket.
    /// </summary>
    /// <param name="TicketId">The unique identifier of the ticket to be reopened.</param>
    public sealed record ReopenTicketCommand(Guid TicketId);
}
