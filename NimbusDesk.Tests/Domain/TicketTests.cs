using NimbusDesk.Domain.Entities;
using NimbusDesk.Domain.Exceptions;
using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace NimbusDesk.Tests.Domain
{
    public class TicketTests
    {
        [Fact]
        public void Creating_ticket_sets_status_to_open()
        {
            // Act
            var ticket = new Ticket(
                "Login issue",
                "User cannot log in",
                TicketPriority.Medium);

            // Assert
            Assert.Equal(TicketStatus.Open, ticket.Status);
        }

        [Fact]
        public void Creating_ticket_sets_created_at()
        {
            // Act
            var before = DateTime.UtcNow;
            var ticket = new Ticket(
                "Payment issue",
                "Payment failed",
                TicketPriority.High);
            var after = DateTime.UtcNow;

            // Assert
            Assert.True(ticket.CreatedAt >= before);
            Assert.True(ticket.CreatedAt <= after);
        }

        [Fact]
        public void Ticket_title_cannot_be_empty()
        {
            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new Ticket(
                    "",
                    "Some description",
                    TicketPriority.Low));
        }

        [Fact]
        public void Open_ticket_can_move_to_in_progress()
        {
            // Arrange
            var ticket = new Ticket(
                "Bug report",
                "Unexpected error",
                TicketPriority.Low);

            // Act
            ticket.StartProgress();

            // Assert
            Assert.Equal(TicketStatus.InProgress, ticket.Status);
        }

        [Fact]
        public void Only_open_ticket_can_move_to_in_progress()
        {
            // Arrange
            var ticket = new Ticket(
                "Bug report",
                "Unexpected error",
                TicketPriority.Low);

            ticket.StartProgress();

            // Act & Assert
            Assert.Throws<DomainException>(() => ticket.StartProgress());
        }

        [Fact]
        public void Closing_ticket_sets_closed_status_and_closed_at()
        {
            // Arrange
            var ticket = new Ticket(
                "Support request",
                "Resolved issue",
                TicketPriority.Medium);

            // Act
            ticket.Close();

            // Assert
            Assert.Equal(TicketStatus.Closed, ticket.Status);
            Assert.NotNull(ticket.ClosedAt);
        }

        [Fact]
        public void Closing_ticket_twice_throws_exception()
        {
            // Arrange
            var ticket = new Ticket(
                "Duplicate issue",
                "Test case",
                TicketPriority.Low);

            ticket.Close();

            // Act & Assert
            Assert.Throws<DomainException>(() => ticket.Close());
        }
    }
}

