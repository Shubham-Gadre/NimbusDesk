using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NimbusDesk.Domain.Entities;

/// <summary>
/// Configures the Entity Framework Core mapping for the TicketHistory entity.
/// Defines table structure, column constraints, indexes, and relationships including cascading deletes.
/// </summary>
public sealed class TicketHistoryConfiguration
    : IEntityTypeConfiguration<TicketHistory>
{
    /// <summary>
    /// Configures the TicketHistory entity mapping to the database.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the TicketHistory entity.</param>
    public void Configure(EntityTypeBuilder<TicketHistory> builder)
    {
        builder.ToTable("TicketHistory");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
               .ValueGeneratedNever();

        builder.Property(h => h.TicketId)
               .IsRequired();

        builder.Property(h => h.ChangeType)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(h => h.FromValue)
               .HasMaxLength(200);

        builder.Property(h => h.ToValue)
               .HasMaxLength(200);

        builder.Property(h => h.ChangedAt)
               .IsRequired();

        builder.HasIndex(h => h.TicketId);

        builder.HasOne<Ticket>()
               .WithMany(t => t.History)
               .HasForeignKey(h => h.TicketId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(h => new { h.TicketId, h.ChangedAt });

    }
}
