using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Assign
{
    /// <summary>
    /// Represents a command to assign a ticket to a user.
    /// </summary>
    /// <param name="TicketId">The unique identifier of the ticket to be assigned.</param>
    /// <param name="UserId">The unique identifier of the user to assign the ticket to.</param>
    public sealed record AssignTicketCommand(Guid TicketId, Guid UserId);

}
