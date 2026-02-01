using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NimbusDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Infrastructure.Persistence.Configurations
{
    public sealed class TicketHistoryConfiguration
    : IEntityTypeConfiguration<TicketHistory>
    {
        public void Configure(EntityTypeBuilder<TicketHistory> builder)
        {
            builder.ToTable("TicketHistory");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id)
                   .ValueGeneratedNever();


            builder.Property(h => h.TicketId)
                .IsRequired();

            builder.Property(h => h.FromStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(h => h.ToStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(h => h.ChangedAt)
                .IsRequired();

            builder.HasIndex(h => h.TicketId);
        }
    }
}
