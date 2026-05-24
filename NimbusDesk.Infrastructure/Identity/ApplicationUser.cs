using Microsoft.AspNetCore.Identity;
using NimbusDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Infrastructure.Identity
{
    /// <summary>
    /// Represents an application user with extended identity information.
    /// Extends ASP.NET Identity's IdentityUser with custom properties for ticket assignment and user profile management.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>Gets or sets the first name of the user.</summary>
        // Custom profile data
        public string FirstName { get; set; } = string.Empty;
        /// <summary>Gets or sets the last name of the user.</summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets the full name of the user by combining first and last names.
        /// Calculated property useful for UI display and ticket assignment lists.
        /// </summary>
        // Calculated property for the UI (very helpful for Ticket Assignment lists)
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>Gets or sets the date and time when the user account was created in UTC.</summary>
        // Professional tracking
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// <summary>Gets or sets a value indicating whether the user account is active.</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the collection of tickets assigned to this user.
        /// Navigation property for Entity Framework Core relationships.
        /// </summary>
        // Navigation properties (Optional, but good for EF performance)
        // This allows you to say: user.AssignedTickets
        public virtual ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
    }
}
