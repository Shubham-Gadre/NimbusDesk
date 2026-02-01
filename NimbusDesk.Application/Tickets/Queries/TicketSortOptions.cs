using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public static class TicketSortOptions
    {
        public const string CreatedAt = "createdAt";
        public const string Priority = "priority";
        public const string Status = "status";

        public static bool IsValid(string value) =>
            value == CreatedAt ||
            value == Priority ||
            value == Status;
    }
}
