using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets
{
    public sealed record CreateTicketCommand
    (
        string Title,
        string Description,
        TicketPriority Priority
    );
}
