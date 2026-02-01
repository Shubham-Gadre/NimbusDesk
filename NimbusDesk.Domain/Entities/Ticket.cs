using NimbusDesk.Domain.Exceptions;
using NimbusDesk.Domain.ValueObjects;

namespace NimbusDesk.Domain.Entities
{
    /// <summary>
    /// Represents a customer issue that moves through a controlled lifecycle.
    /// The Ticket entity is responsible for enforcing its own invariants
    /// and preventing invalid state transitions.
    /// </summary>
    public class Ticket
    {

        private readonly List<TicketHistory> _history = new();

        public IReadOnlyCollection<TicketHistory> History => _history.AsReadOnly();

        public Guid Id { get; }

        /// <summary>
        /// Short, human-readable summary of the issue.
        /// Cannot be empty.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Detailed description of the problem.
        /// Optional but encouraged.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Current lifecycle status of the ticket.
        /// Status transitions are controlled by domain methods.
        /// </summary>
        public TicketStatus Status { get; private set; }

        /// <summary>
        /// Indicates urgency and impact.
        /// </summary>
        public TicketPriority Priority { get; private set; }

        /// <summary>
        /// UTC timestamp when the ticket was created.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// UTC timestamp when the ticket was closed.
        /// Set only when status becomes Closed.
        /// </summary>
        public DateTime? ClosedAt { get; private set; }

        /// <summary>
        /// Creates a new ticket.
        /// Invariant: A ticket always starts in the Open state.
        /// </summary>
        public Ticket(string title, string description, TicketPriority priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Ticket title cannot be empty.");

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Priority = priority;
            Status = TicketStatus.Open;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Moves the ticket from Open to In Progress.
        /// </summary>
        public void StartProgress()
        {
            if (Status != TicketStatus.Open)
                throw new DomainException(
                    "Only tickets in Open state can be moved to In Progress.");

            Status = TicketStatus.InProgress;
        }

        /// <summary>
        /// Marks the ticket as Waiting (e.g. for customer input).
        /// Allowed only from In Progress.
        /// </summary>
        public void MarkWaiting()
        {
            if (Status != TicketStatus.InProgress)
                throw new DomainException(
                    "Only tickets in progress can be marked as Waiting.");

            Status = TicketStatus.Waiting;
        }

        /// <summary>
        /// Closes the ticket permanently.
        /// Once closed, no further state changes are allowed.
        /// </summary>
        public void Close()
        {
            if (Status == TicketStatus.Closed)
                throw new DomainException("Ticket is already closed.");

            var previousStatus = Status.Value;

            Status = TicketStatus.Closed;
            ClosedAt = DateTime.UtcNow;
            var history = TicketHistory.Create(
                    Id,
                    previousStatus,
                    Status.Value,
                    DateTime.UtcNow);
            _history.Add(history);

        }

    }
}

