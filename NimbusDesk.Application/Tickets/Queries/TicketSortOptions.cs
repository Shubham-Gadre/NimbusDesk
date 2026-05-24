using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    /// <summary>
    /// Defines the available sort options for ticket queries.
    /// Provides constants for valid sort field names and a validation method.
    /// </summary>
    public static class TicketSortOptions
    {
        /// <summary>Sort by ticket creation date.</summary>
        public const string CreatedAt = "createdAt";
        /// <summary>Sort by ticket priority level.</summary>
        public const string Priority = "priority";
        /// <summary>Sort by ticket status.</summary>
        public const string Status = "status";

        /// <summary>
        /// Validates whether the provided value is a valid sort option.
        /// </summary>
        /// <param name="value">The sort option value to validate.</param>
        /// <returns>True if the value is a valid sort option; otherwise, false.</returns>
        public static bool IsValid(string value) =>
            value == CreatedAt ||
            value == Priority ||
            value == Status;
    }
}
