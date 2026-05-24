using NimbusDesk.Domain.Entities;
using NimbusDesk.Domain.Exceptions;
using NimbusDesk.Domain.ValueObjects;

namespace NimbusDesk.Domain.Entities
{
    /// <summary>
    /// Represents a ticket in the support management system.
    /// Manages ticket lifecycle including creation, assignment, status changes, comments, and history tracking.
    /// </summary>
    public class Ticket
    {
        private readonly List<TicketHistory> _history = new();
        /// <summary>
        /// Gets the read-only collection of historical changes made to this ticket.
        /// </summary>
        public IReadOnlyCollection<TicketHistory> History => _history.AsReadOnly();

        /// <summary>Gets the unique identifier of the ticket.</summary>
        public Guid Id { get; }
        /// <summary>Gets or sets the title of the ticket.</summary>
        public string Title { get; private set; }
        /// <summary>Gets or sets the detailed description of the ticket.</summary>
        public string Description { get; private set; }
        /// <summary>Gets or sets the current status of the ticket (Open or Closed).</summary>
        public TicketStatus Status { get; private set; }
        /// <summary>Gets or sets the priority level of the ticket.</summary>
        public TicketPriority Priority { get; private set; }
        /// <summary>Gets the date and time when the ticket was created in UTC.</summary>
        public DateTime CreatedAt { get; }
        /// <summary>Gets or sets the date and time when the ticket was closed, or null if still open.</summary>
        public DateTime? ClosedAt { get; private set; }
        /// <summary>Gets or sets the row version for optimistic concurrency control.</summary>
        public byte[] RowVersion { get; private set; }
        /// <summary>Gets or sets the user ID the ticket is assigned to, or null if unassigned.</summary>
        public Guid? AssignedToUserId { get; private set; }

        private readonly List<Comment> _comments = new();
        /// <summary>
        /// Gets the read-only collection of comments added to this ticket.
        /// </summary>
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        /// <summary>
        /// Adds a comment to the ticket.
        /// </summary>
        /// <param name="userId">The ID of the user adding the comment.</param>
        /// <param name="content">The content of the comment.</param>
        /// <exception cref="DomainException">Thrown when attempting to add a comment to a closed ticket.</exception>
        public void AddComment(Guid userId, string content)
        {
            // Business Rule: Accountability and Logic
            if (Status == TicketStatus.Closed)
                throw new DomainException("Cannot add comments to a closed ticket.");

            var comment = new Comment(Id, userId, content);
            _comments.Add(comment);
        }

        /// <summary>
        /// Assigns the ticket to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to assign the ticket to.</param>
        /// <exception cref="DomainException">Thrown when attempting to assign a closed ticket.</exception>
        public void Assign(Guid userId)
        {
            if (Status == TicketStatus.Closed)
                throw new DomainException("Cannot assign a closed ticket.");

            if (AssignedToUserId == userId) return;

            var fromValue = AssignedToUserId?.ToString() ?? "Unassigned";
            AssignedToUserId = userId;

            // Use the static Create method instead of 'new'
            _history.Add(TicketHistory.Create(
                Id,
                "AssignmentChanged",
                fromValue,
                userId.ToString()));
        }

        /// <summary>
        /// Initializes a new ticket with the specified title, description, and priority.
        /// </summary>
        /// <param name="title">The title of the ticket (cannot be empty).</param>
        /// <param name="description">The description of the ticket.</param>
        /// <param name="priority">The priority level of the ticket.</param>
        /// <exception cref="DomainException">Thrown when the title is null, empty, or whitespace.</exception>
        public Ticket(string title, string description, TicketPriority priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Ticket title cannot be empty.");

            Id = Guid.NewGuid();
            Title = title.Trim();
            Description = description?.Trim();
            Priority = priority;
            Status = TicketStatus.Open;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Closes the ticket and records the closure time.
        /// </summary>
        /// <exception cref="DomainException">Thrown when the ticket is already closed.</exception>
        public void Close()
        {
            if (Status == TicketStatus.Closed)
                throw new DomainException("Ticket is already closed.");

            _history.Add(TicketHistory.Create(
                Id,
                "StatusChanged",
                Status.Value,
                TicketStatus.Closed.Value));

            Status = TicketStatus.Closed;
            ClosedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Reopens a closed ticket.
        /// </summary>
        /// <exception cref="DomainException">Thrown when attempting to reopen a ticket that is not closed.</exception>
        public void Reopen()
        {
            if (Status != TicketStatus.Closed)
                throw new DomainException("Only closed tickets can be reopened.");

            _history.Add(TicketHistory.Create(
                Id,
                "StatusChanged",
                Status.Value,
                TicketStatus.Open.Value));

            Status = TicketStatus.Open;
            ClosedAt = null;
        }

        /// <summary>
        /// Updates the ticket's details (title, description, and priority).
        /// Creates history records for any changed fields.
        /// </summary>
        /// <param name="title">The new title for the ticket.</param>
        /// <param name="description">The new description for the ticket.</param>
        /// <param name="priority">The new priority level for the ticket.</param>
        /// <exception cref="DomainException">Thrown when attempting to update a closed ticket.</exception>
        public void UpdateDetails(string title, string description, TicketPriority priority)
        {
            if (Status == TicketStatus.Closed)
                throw new DomainException("Closed tickets cannot be updated.");

            title = title?.Trim();
            description = description?.Trim();

            if (Title != title)
            {
                _history.Add(TicketHistory.Create(
                    Id,
                    "TitleChanged",
                    Title,
                    title));

                Title = title;
            }

            if (Description != description)
            {
                _history.Add(TicketHistory.Create(
                    Id,
                    "DescriptionChanged",
                    Description,
                    description));

                Description = description;
            }

            if (Priority != priority)
            {
                _history.Add(TicketHistory.Create(
                    Id,
                    "PriorityChanged",
                    Priority.Value,
                    priority.Value));

                Priority = priority;
            }
        }
    }
}
