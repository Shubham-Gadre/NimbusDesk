using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NimbusDesk.Domain.Entities;
using NimbusDesk.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Infrastructure.Persistence.Configurations
{
    public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasMaxLength(2000);

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.ClosedAt);

            // Value Object: TicketStatus
            builder.Property(t => t.Status)
                .HasConversion(
                    status => status.Value,
                      value => TicketStatus.FromValue(value))
                .IsRequired()
                .HasMaxLength(50)
                .IsConcurrencyToken(false);

            // Value Object: TicketPriority
            builder.Property(t => t.Priority)
                .HasConversion(
                    priority => priority.Value,
                    value => TicketPriority.FromValue(value))
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => t.CreatedAt);

            // Composite index for status filtering + pagination
            builder.HasIndex("Status", nameof(Ticket.CreatedAt))
                   .HasDatabaseName("IX_Tickets_Status_CreatedAt");

            // Composite index for priority filtering + pagination
            builder.HasIndex("Priority", nameof(Ticket.CreatedAt))
                   .HasDatabaseName("IX_Tickets_Priority_CreatedAt");

            // Index for default ordering
            builder.HasIndex(t => t.CreatedAt)
                   .HasDatabaseName("IX_Tickets_CreatedAt");

            builder.HasMany(t => t.History)
                   .WithOne()
                   .HasForeignKey(h => h.TicketId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(t => t.History)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);


        }
    }
}
