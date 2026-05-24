using NimbusDesk.Domain.Exceptions;

namespace NimbusDesk.Domain.Entities
{
    /// <summary>
    /// Represents a historical record of changes made to a ticket.
    /// Each instance tracks what changed, from what value to what value, and when the change occurred.
    /// </summary>
    public sealed class TicketHistory
    {
        /// <summary>Gets the unique identifier of this history record.</summary>
        public Guid Id { get; private set; }
        /// <summary>Gets the ID of the ticket this history record belongs to.</summary>
        public Guid TicketId { get; private set; }

        /// <summary>Gets the type of change (e.g., "StatusChanged", "TitleChanged", "AssignmentChanged").</summary>
        public string ChangeType { get; private set; }
        /// <summary>Gets the previous value before the change.</summary>
        public string FromValue { get; private set; }
        /// <summary>Gets the new value after the change.</summary>
        public string ToValue { get; private set; }

        /// <summary>Gets the date and time when the change was recorded in UTC.</summary>
        public DateTime ChangedAt { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketHistory"/> class.
        /// This constructor is for Entity Framework Core use only.
        /// </summary>
        private TicketHistory() { } // EF Core

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketHistory"/> class with the specified parameters.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket this history record belongs to.</param>
        /// <param name="changeType">The type of change that occurred.</param>
        /// <param name="fromValue">The value before the change.</param>
        /// <param name="toValue">The value after the change.</param>
        private TicketHistory(
            Guid ticketId,
            string changeType,
            string fromValue,
            string toValue)
        {
            Id = Guid.NewGuid();
            TicketId = ticketId;
            ChangeType = changeType;
            FromValue = fromValue;
            ToValue = toValue;
            ChangedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Creates a new <see cref="TicketHistory"/> record with validation.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket this history record belongs to.</param>
        /// <param name="changeType">The type of change (cannot be empty or whitespace).</param>
        /// <param name="fromValue">The value before the change.</param>
        /// <param name="toValue">The value after the change.</param>
        /// <returns>A new <see cref="TicketHistory"/> instance.</returns>
        /// <exception cref="DomainException">Thrown when changeType is null, empty, or whitespace.</exception>
        public static TicketHistory Create(
            Guid ticketId,
            string changeType,
            string fromValue,
            string toValue)
        {
            if (string.IsNullOrWhiteSpace(changeType))
                throw new DomainException("ChangeType cannot be empty.");

            return new TicketHistory(
                ticketId,
                changeType,
                fromValue,
                toValue);
        }
    }
}


