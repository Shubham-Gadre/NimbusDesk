using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Create
{
    /// <summary>
    /// Represents a command to create a new ticket.
    /// </summary>
    /// <param name="Title">The title of the ticket to be created (cannot be empty).</param>
    /// <param name="Description">The detailed description of the ticket.</param>
    /// <param name="Priority">The priority level of the ticket ("Low", "Medium", or "High").</param>
    public sealed record CreateTicketCommand
    (
        string Title,
        string Description,
        string Priority
    );
}
