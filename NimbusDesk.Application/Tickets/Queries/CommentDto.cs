using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public sealed record CommentDto(
    Guid Id,
    Guid UserId,
    string Content,
    DateTime CreatedAt);
}
