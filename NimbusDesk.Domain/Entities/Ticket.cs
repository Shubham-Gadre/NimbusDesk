using NimbusDesk.Domain.Entities;
using NimbusDesk.Domain.Exceptions;
using NimbusDesk.Domain.ValueObjects;

namespace NimbusDesk.Domain.Entities
{
    public class Ticket
    {
        private readonly List<TicketHistory> _history = new();
        public IReadOnlyCollection<TicketHistory> History => _history.AsReadOnly();

        public Guid Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TicketStatus Status { get; private set; }
        public TicketPriority Priority { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime? ClosedAt { get; private set; }




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
