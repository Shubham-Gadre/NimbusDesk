using FluentValidation;
using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets
{
    public sealed class CreateTicketValidator
    : AbstractValidator<CreateTicketCommand>
    {
        public CreateTicketValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.Priority)
                .NotNull()
                .Must(BeValidPriority)
                .WithMessage("Invalid ticket priority.");
        }

        private static bool BeValidPriority(TicketPriority priority)
        {
            return priority == TicketPriority.Low
                || priority == TicketPriority.Medium
                || priority == TicketPriority.High;
        }
    }
}
