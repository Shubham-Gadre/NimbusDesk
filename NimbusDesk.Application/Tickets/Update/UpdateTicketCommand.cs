using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Update
{
    /// <summary>
    /// Represents a command to update an existing ticket's details.
    /// </summary>
    /// <param name="TicketId">The unique identifier of the ticket to be updated.</param>
    /// <param name="Title">The new title for the ticket.</param>
    /// <param name="Description">The new description for the ticket.</param>
    /// <param name="Priority">The new priority level for the ticket ("Low", "Medium", or "High").</param>
    public sealed record UpdateTicketCommand(
    Guid TicketId,
    string Title,
    string Description,
    string Priority);
}
