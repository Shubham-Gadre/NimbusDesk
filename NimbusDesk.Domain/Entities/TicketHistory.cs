using NimbusDesk.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Domain.Entities
{
    public sealed class TicketHistory
    {
        public Guid Id { get; private set; }
        public Guid TicketId { get; private set; }

        public string FromStatus { get; private set; }
        public string ToStatus { get; private set; }

        public DateTime ChangedAt { get; private set; }

        // EF Core constructor
        private TicketHistory() { }

        private TicketHistory(
            Guid ticketId,
            string fromStatus,
            string toStatus,
            DateTime changedAt)
        {
            Id = Guid.NewGuid();
            TicketId = ticketId;
            FromStatus = fromStatus;
            ToStatus = toStatus;
            ChangedAt = changedAt;
        }

        public static TicketHistory Create(
            Guid ticketId,
            string fromStatus,
            string toStatus,
            DateTime changedAt)
        {
            if (string.IsNullOrWhiteSpace(fromStatus))
                throw new DomainException("FromStatus cannot be empty.");

            if (string.IsNullOrWhiteSpace(toStatus))
                throw new DomainException("ToStatus cannot be empty.");

            return new TicketHistory(
                ticketId,
                fromStatus,
                toStatus,
                changedAt);
        }
    }
}
