using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Domain.ValueObjects
{
    /// <summary>
    /// Represents the priority level of a ticket using a value object pattern.
    /// Provides predefined priority levels: Low, Medium, and High.
    /// </summary>
    public sealed class TicketPriority
    {
        /// <summary>Gets the Low priority level.</summary>
        public static readonly TicketPriority Low = new("Low");
        /// <summary>Gets the Medium priority level.</summary>
        public static readonly TicketPriority Medium = new("Medium");
        /// <summary>Gets the High priority level.</summary>
        public static readonly TicketPriority High = new("High");

        /// <summary>Gets the string value representing the priority level.</summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketPriority"/> class.
        /// </summary>
        /// <param name="value">The string representation of the priority level.</param>
        private TicketPriority(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a <see cref="TicketPriority"/> instance from a string value.
        /// </summary>
        /// <param name="value">The string value representing a priority level ("Low", "Medium", or "High").</param>
        /// <returns>The corresponding <see cref="TicketPriority"/> instance.</returns>
        /// <exception cref="DomainException">Thrown when the value does not match any valid priority level.</exception>
        public static TicketPriority FromValue(string value)
        {
            return value switch
            {
                "Low" => Low,
                "Medium" => Medium,
                "High" => High,
                _ => throw new DomainException($"Invalid ticket priority: {value}")
            };
        }
    }
}
