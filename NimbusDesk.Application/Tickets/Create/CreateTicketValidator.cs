using FluentValidation;
using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Create
{
    /// <summary>
    /// Validator for the CreateTicketCommand.
    /// Validates ticket title, description, and priority according to business rules.
    /// </summary>
    public sealed class CreateTicketValidator
    : AbstractValidator<CreateTicketCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTicketValidator"/> class.
        /// Configures validation rules for ticket creation.
        /// </summary>
        public CreateTicketValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.Priority)
            .NotEmpty()
            .Must(BeValidPriority)
            .WithMessage("Invalid ticket priority.");
        }

        /// <summary>
        /// Validates that the provided priority is one of the acceptable values.
        /// </summary>
        /// <param name="priority">The priority value to validate.</param>
        /// <returns>True if the priority is valid (Low, Medium, or High); otherwise, false.</returns>
        private static bool BeValidPriority(string priority)
        {
            return priority == "Low"
                || priority == "Medium"
                || priority == "High";
        }
        
    }
}
