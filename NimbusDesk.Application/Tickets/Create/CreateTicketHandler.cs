using FluentValidation;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Entities;
using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Create
{
    public sealed class CreateTicketHandler
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IValidator<CreateTicketCommand> _validator;

        public CreateTicketHandler(
            ITicketRepository ticketRepository,
            IValidator<CreateTicketCommand> validator)
        {
            _ticketRepository = ticketRepository;
            _validator = validator;
        }

        public async Task<Guid> Handle(
            CreateTicketCommand command,
            CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(command, cancellationToken);
            var priority = TicketPriority.FromValue(command.Priority);

            var ticket = new Ticket(
                command.Title,
                command.Description,
                priority);

            await _ticketRepository.AddAsync(ticket, cancellationToken);

            return ticket.Id;
        }
    }
}
