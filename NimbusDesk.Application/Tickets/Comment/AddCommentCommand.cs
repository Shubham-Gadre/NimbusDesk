using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Comment
{
    public sealed record AddCommentCommand(Guid TicketId, Guid UserId, string Content);
}
