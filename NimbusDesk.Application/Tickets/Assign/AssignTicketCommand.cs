using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Assign
{
    public sealed record AssignTicketCommand(Guid TicketId, Guid UserId);

}
