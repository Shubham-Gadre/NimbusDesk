using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Create
{
    public sealed record CreateTicketCommand
    (
        string Title,
        string Description,
        string Priority
    );
}
