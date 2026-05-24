using FluentValidation;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Domain.Entities;
using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Create
{
    /// <summary>
    /// Handles the creation of a new ticket.
    /// Validates the command, creates a domain ticket entity, and persists it to the database.
    /// </summary>
    public sealed class CreateTicketHandler
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IValidator<CreateTicketCommand> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTicketHandler"/> class.
        /// </summary>
        /// <param name="ticketRepository">The repository for ticket persistence operations.</param>
        /// <param name="validator">The validator for CreateTicketCommand validation.</param>
        public CreateTicketHandler(
            ITicketRepository ticketRepository,
            IValidator<CreateTicketCommand> validator)
        {
            _ticketRepository = ticketRepository;
            _validator = validator;
        }

        /// <summary>
        /// Handles the creation of a new ticket from the command.
        /// </summary>
        /// <param name="command">The command containing ticket creation parameters.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The unique identifier of the created ticket.</returns>
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
