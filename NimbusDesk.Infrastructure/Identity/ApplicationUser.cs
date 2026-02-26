using Microsoft.AspNetCore.Identity;
using NimbusDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Custom profile data
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Calculated property for the UI (very helpful for Ticket Assignment lists)
        public string FullName => $"{FirstName} {LastName}";

        // Professional tracking
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation properties (Optional, but good for EF performance)
        // This allows you to say: user.AssignedTickets
        public virtual ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
    }
}
