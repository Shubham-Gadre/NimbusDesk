using NimbusDesk.Domain.Entities;
using NimbusDesk.Domain.Exceptions;
using NimbusDesk.Domain.ValueObjects;
using System;
using System.Linq;
using Xunit;

namespace NimbusDesk.Tests.Domain
{
    public class TicketTests
    {
        [Fact]
        public void Constructor_Throws_When_Title_Is_Null_Or_Whitespace()
        {
            Assert.Throws<DomainException>(() => new Ticket(null, "desc", TicketPriority.Low));
            Assert.Throws<DomainException>(() => new Ticket("", "desc", TicketPriority.Low));
            Assert.Throws<DomainException>(() => new Ticket("   ", "desc", TicketPriority.Low));
        }

        [Fact]
        public void Constructor_Trims_Title_And_Description_And_Sets_Properties()
        {
            var ticket = new Ticket("  Title  ", "  Desc  ", TicketPriority.High);

            Assert.NotEqual(Guid.Empty, ticket.Id);
            Assert.Equal("Title", ticket.Title);
            Assert.Equal("Desc", ticket.Description);
            Assert.Equal(TicketPriority.High, ticket.Priority);
            Assert.Equal(TicketStatus.Open, ticket.Status);
            Assert.True(ticket.CreatedAt > DateTime.MinValue);
            Assert.Null(ticket.ClosedAt);
            Assert.Empty(ticket.History);
        }

        [Fact]
        public void Close_Sets_Status_Closed_Adds_History_And_Sets_ClosedAt()
        {
            var ticket = new Ticket("t", "d", TicketPriority.Low);

            ticket.Close();

            Assert.Equal(TicketStatus.Closed, ticket.Status);
            Assert.NotNull(ticket.ClosedAt);

            var hist = ticket.History.Last();
            Assert.Equal("StatusChanged", hist.ChangeType);
            Assert.Equal(TicketStatus.Open.Value, hist.FromValue);
            Assert.Equal(TicketStatus.Closed.Value, hist.ToValue);
            Assert.Equal(ticket.Id, hist.TicketId);
            Assert.True(hist.ChangedAt > DateTime.MinValue);
        }

        [Fact]
        public void Close_Throws_When_Already_Closed()
        {
            var ticket = new Ticket("t", "d", TicketPriority.Low);
            ticket.Close();
            Assert.Throws<DomainException>(() => ticket.Close());
        }

        [Fact]
        public void Reopen_Throws_When_Not_Closed()
        {
            var ticket = new Ticket("t", "d", TicketPriority.Low);
            Assert.Throws<DomainException>(() => ticket.Reopen());
        }

        [Fact]
        public void Reopen_Sets_Status_Open_Adds_History_And_Clears_ClosedAt()
        {
            var ticket = new Ticket("t", "d", TicketPriority.Low);
            ticket.Close();
            ticket.Reopen();

            Assert.Equal(TicketStatus.Open, ticket.Status);
            Assert.Null(ticket.ClosedAt);

            var hist = ticket.History.Last();
            Assert.Equal("StatusChanged", hist.ChangeType);
            Assert.Equal(TicketStatus.Closed.Value, hist.FromValue);
            Assert.Equal(TicketStatus.Open.Value, hist.ToValue);
        }

        [Fact]
        public void UpdateDetails_Throws_When_Closed()
        {
            var ticket = new Ticket("t", "d", TicketPriority.Low);
            ticket.Close();
            Assert.Throws<DomainException>(() => ticket.UpdateDetails("new", "new", TicketPriority.Medium));
        }

        [Fact]
        public void UpdateDetails_Changes_Values_And_Adds_History_Entries()
        {
            var ticket = new Ticket("Title", "Desc", TicketPriority.Low);
            var beforeCount = ticket.History.Count;

            ticket.UpdateDetails("  NewTitle  ", "  NewDesc  ", TicketPriority.High);

            Assert.Equal("NewTitle", ticket.Title);
            Assert.Equal("NewDesc", ticket.Description);
            Assert.Equal(TicketPriority.High, ticket.Priority);

            Assert.Equal(beforeCount + 3, ticket.History.Count);

            var lastThree = ticket.History.Skip(Math.Max(0, ticket.History.Count - 3)).ToArray();

            Assert.Equal("TitleChanged", lastThree[0].ChangeType);
            Assert.Equal("Title", lastThree[0].FromValue);
            Assert.Equal("NewTitle", lastThree[0].ToValue);

            Assert.Equal("DescriptionChanged", lastThree[1].ChangeType);
            Assert.Equal("Desc", lastThree[1].FromValue);
            Assert.Equal("NewDesc", lastThree[1].ToValue);

            Assert.Equal("PriorityChanged", lastThree[2].ChangeType);
            Assert.Equal(TicketPriority.Low.Value, lastThree[2].FromValue);
            Assert.Equal(TicketPriority.High.Value, lastThree[2].ToValue);
        }

        [Fact]
        public void UpdateDetails_NoChanges_Does_Not_Add_History()
        {
            var ticket = new Ticket("T", "D", TicketPriority.Low);
            var before = ticket.History.Count;

            ticket.UpdateDetails("T", "D", TicketPriority.Low);

            Assert.Equal(before, ticket.History.Count);
        }

        [Fact]
        public void Constructor_Allows_Null_Description()
        {
            var ticket = new Ticket("Title", null, TicketPriority.Low);
            Assert.Null(ticket.Description);
            Assert.Empty(ticket.History);
        }

        [Fact]
        public void UpdateDetails_OnlyTitle_Adds_Single_TitleChanged_History()
        {
            var ticket = new Ticket("OldTitle", "Desc", TicketPriority.Low);
            var before = ticket.History.Count;

            ticket.UpdateDetails("  NewTitle  ", "Desc", TicketPriority.Low);

            Assert.Equal("NewTitle", ticket.Title);
            Assert.Equal(before + 1, ticket.History.Count);

            var last = ticket.History.Last();
            Assert.Equal("TitleChanged", last.ChangeType);
            Assert.Equal("OldTitle", last.FromValue);
            Assert.Equal("NewTitle", last.ToValue);
        }

        [Fact]
        public void UpdateDetails_OnlyDescription_Adds_Single_DescriptionChanged_History()
        {
            var ticket = new Ticket("Title", "OldDesc", TicketPriority.Low);
            var before = ticket.History.Count;

            ticket.UpdateDetails("Title", "  NewDesc  ", TicketPriority.Low);

            Assert.Equal("NewDesc", ticket.Description);
            Assert.Equal(before + 1, ticket.History.Count);

            var last = ticket.History.Last();
            Assert.Equal("DescriptionChanged", last.ChangeType);
            Assert.Equal("OldDesc", last.FromValue);
            Assert.Equal("NewDesc", last.ToValue);
        }

        [Fact]
        public void UpdateDetails_OnlyPriority_Adds_Single_PriorityChanged_History()
        {
            var ticket = new Ticket("Title", "Desc", TicketPriority.Low);
            var before = ticket.History.Count;

            ticket.UpdateDetails("Title", "Desc", TicketPriority.High);

            Assert.Equal(TicketPriority.High, ticket.Priority);
            Assert.Equal(before + 1, ticket.History.Count);

            var last = ticket.History.Last();
            Assert.Equal("PriorityChanged", last.ChangeType);
            Assert.Equal(TicketPriority.Low.Value, last.FromValue);
            Assert.Equal(TicketPriority.High.Value, last.ToValue);
        }

        [Fact]
        public void UpdateDetails_TrimmedValues_That_Are_Equal_Do_Not_Add_History()
        {
            var ticket = new Ticket("T", "D", TicketPriority.Low);
            var before = ticket.History.Count;

            // Inputs that trim to the same existing values
            ticket.UpdateDetails("  T  ", "  D  ", TicketPriority.Low);

            Assert.Equal("T", ticket.Title);
            Assert.Equal("D", ticket.Description);
            Assert.Equal(before, ticket.History.Count);
        }

        [Fact]
        public void History_Is_ReadOnly_And_TicketId_Is_Preserved_For_All_Entries()
        {
            var ticket = new Ticket("Title", "Desc", TicketPriority.Low);

            // create three changes to populate history
            ticket.UpdateDetails("NewTitle", "Desc", TicketPriority.Low);
            ticket.UpdateDetails("NewTitle", "NewDesc", TicketPriority.Low);
            ticket.UpdateDetails("NewTitle", "NewDesc", TicketPriority.High);

            // All history records should reference the ticket Id
            Assert.All(ticket.History, h => Assert.Equal(ticket.Id, h.TicketId));

            // History collection should be read-only (mutating operations throw)
            var coll = (ICollection<TicketHistory>)ticket.History;
            Assert.True(coll.IsReadOnly);
            Assert.Throws<NotSupportedException>(() => coll.Clear());
        }

    }
}

