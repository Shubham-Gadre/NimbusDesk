using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Domain.ValueObjects
{
    /// <summary>
    /// Represents the status of a ticket using a value object pattern.
    /// Provides predefined status levels: Open, InProgress, Waiting, and Closed.
    /// </summary>
    public sealed class TicketStatus
    {
        /// <summary>Gets the Open status level.</summary>
        public static readonly TicketStatus Open = new("Open");
        /// <summary>Gets the InProgress status level.</summary>
        public static readonly TicketStatus InProgress = new("InProgress");
        /// <summary>Gets the Waiting status level.</summary>
        public static readonly TicketStatus Waiting = new("Waiting");
        /// <summary>Gets the Closed status level.</summary>
        public static readonly TicketStatus Closed = new("Closed");

        /// <summary>Gets the string value representing the status level.</summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketStatus"/> class.
        /// </summary>
        /// <param name="value">The string representation of the status level.</param>
        private TicketStatus(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a <see cref="TicketStatus"/> instance from a string value.
        /// </summary>
        /// <param name="value">The string value representing a status level ("Open", "InProgress", "Waiting", or "Closed").</param>
        /// <returns>The corresponding <see cref="TicketStatus"/> instance.</returns>
        /// <exception cref="DomainException">Thrown when the value does not match any valid status level.</exception>
        public static TicketStatus FromValue(string value)
        {
            return value switch
            {
                "Open" => Open,
                "InProgress" => InProgress,
                "Waiting" => Waiting,
                "Closed" => Closed,
                _ => throw new DomainException($"Invalid ticket status: {value}")
            };
        }
    }
}
