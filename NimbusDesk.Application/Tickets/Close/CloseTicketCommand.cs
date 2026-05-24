using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Close
{
    /// <summary>
    /// Represents a command to close an existing ticket.
    /// </summary>
    /// <param name="TicketId">The unique identifier of the ticket to be closed.</param>
    public sealed record CloseTicketCommand(Guid TicketId);
}
