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
                .HasMaxLength(50);

            // Value Object: TicketPriority
            builder.Property(t => t.Priority)
                .HasConversion(
                    priority => priority.Value,
                    value => TicketPriority.FromValue(value))
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => t.CreatedAt);
        }
    }
}
