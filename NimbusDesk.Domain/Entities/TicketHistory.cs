using NimbusDesk.Domain.Exceptions;

namespace NimbusDesk.Domain.Entities
{
    public sealed class TicketHistory
    {
        public Guid Id { get; private set; }
        public Guid TicketId { get; private set; }

        public string ChangeType { get; private set; }
        public string FromValue { get; private set; }
        public string ToValue { get; private set; }

        public DateTime ChangedAt { get; private set; }

        private TicketHistory() { } // EF Core

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


