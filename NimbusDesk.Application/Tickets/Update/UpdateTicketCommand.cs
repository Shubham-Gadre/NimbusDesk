using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Update
{
    public sealed record UpdateTicketCommand(
    Guid TicketId,
    string Title,
    string Description,
    string Priority);
}
