using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.ReOpen
{
    public sealed record ReopenTicketCommand(Guid TicketId);
}
