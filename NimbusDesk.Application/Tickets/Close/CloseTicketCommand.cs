using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Close
{
    public sealed record CloseTicketCommand(Guid TicketId);
}
