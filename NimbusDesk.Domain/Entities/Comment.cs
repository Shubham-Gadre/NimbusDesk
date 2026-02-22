using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Domain.Entities
{
    public sealed class Comment
    {
        public Guid Id { get; private set; }
        public Guid TicketId { get; private set; }
        public Guid UserId { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Comment() { } // EF Core

        internal Comment(Guid ticketId, Guid userId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new DomainException("Comment content cannot be empty.");

            Id = Guid.NewGuid();
            TicketId = ticketId;
            UserId = userId;
            Content = content.Trim();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
