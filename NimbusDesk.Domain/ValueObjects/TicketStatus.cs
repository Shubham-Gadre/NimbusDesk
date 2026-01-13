using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Domain.ValueObjects
{
    public sealed class TicketStatus
    {
        public static readonly TicketStatus Open = new("Open");
        public static readonly TicketStatus InProgress = new("InProgress");
        public static readonly TicketStatus Waiting = new("Waiting");
        public static readonly TicketStatus Closed = new("Closed");

        public string Value { get; }

        private TicketStatus(string value)
        {
            Value = value;
        }

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
