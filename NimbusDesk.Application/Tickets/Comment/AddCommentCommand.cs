using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Comment
{
    /// <summary>
    /// Represents a command to add a comment to a ticket.
    /// </summary>
    /// <param name="TicketId">The unique identifier of the ticket to add a comment to.</param>
    /// <param name="UserId">The unique identifier of the user adding the comment.</param>
    /// <param name="Content">The content of the comment (cannot be empty or whitespace).</param>
    public sealed record AddCommentCommand(Guid TicketId, Guid UserId, string Content);
}
