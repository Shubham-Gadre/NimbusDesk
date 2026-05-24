using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    /// <summary>
    /// Represents a comment on a ticket for display purposes.
    /// Contains comment content and metadata such as author and creation time.
    /// </summary>
    /// <param name="Id">The unique identifier of the comment.</param>
    /// <param name="UserId">The unique identifier of the user who created the comment.</param>
    /// <param name="Content">The content of the comment.</param>
    /// <param name="CreatedAt">The date and time when the comment was created.</param>
    public sealed record CommentDto(
    Guid Id,
    Guid UserId,
    string Content,
    DateTime CreatedAt);
}
