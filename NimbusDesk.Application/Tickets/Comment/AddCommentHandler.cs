using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Comment
{
    public sealed class AddCommentHandler(ITicketRepository repository)
    {
        public async Task HandleAsync(AddCommentCommand command, CancellationToken ct)
        {
            var ticket = await repository.GetByIdAsync(command.TicketId, ct)
                ?? throw new DomainException("Ticket not found.");

            ticket.AddComment(command.UserId, command.Content);

            await repository.UpdateAsync(ticket, ct);
        }
    }
}
