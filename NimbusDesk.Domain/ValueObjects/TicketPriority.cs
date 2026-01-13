using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Domain.ValueObjects
{
    public sealed class TicketPriority
    {
        public static readonly TicketPriority Low = new("Low");
        public static readonly TicketPriority Medium = new("Medium");
        public static readonly TicketPriority High = new("High");

        public string Value { get; }

        private TicketPriority(string value)
        {
            Value = value;
        }

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
