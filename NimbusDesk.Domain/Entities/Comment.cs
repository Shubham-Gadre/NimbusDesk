using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Domain.Entities
{
    /// <summary>
    /// Represents a comment on a ticket.
    /// Comments provide a way for users to discuss and provide updates on ticket issues.
    /// </summary>
    public sealed class Comment
    {
        /// <summary>Gets the unique identifier of the comment.</summary>
        public Guid Id { get; private set; }
        /// <summary>Gets the ID of the ticket this comment belongs to.</summary>
        public Guid TicketId { get; private set; }
        /// <summary>Gets the ID of the user who created this comment.</summary>
        public Guid UserId { get; private set; }
        /// <summary>Gets the content of the comment.</summary>
        public string Content { get; private set; }
        /// <summary>Gets the date and time when the comment was created in UTC.</summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// This constructor is for Entity Framework Core use only.
        /// </summary>
        private Comment() { } // EF Core

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class with the specified ticket, user, and content.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket this comment belongs to.</param>
        /// <param name="userId">The ID of the user creating the comment.</param>
        /// <param name="content">The content of the comment (cannot be empty or whitespace).</param>
        /// <exception cref="DomainException">Thrown when the content is null, empty, or whitespace.</exception>
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
