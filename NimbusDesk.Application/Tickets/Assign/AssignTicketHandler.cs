using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Assign
{
    public sealed class AssignTicketHandler(ITicketRepository repository)
    {
        public async Task HandleAsync(AssignTicketCommand command, CancellationToken ct)
        {
            var ticket = await repository.GetByIdAsync(command.TicketId, ct)
                ?? throw new DomainException("Ticket not found.");

            ticket.Assign(command.UserId);

            await repository.UpdateAsync(ticket, ct);
        }
    }
}
