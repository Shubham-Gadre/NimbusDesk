using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NimbusDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the Entity Framework Core mapping for the Comment entity.
    /// Defines table structure, column constraints, and indexes.
    /// </summary>
    public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        /// <summary>
        /// Configures the Comment entity mapping to the database.
        /// </summary>
        /// <param name="builder">The entity type builder used to configure the Comment entity.</param>
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(2000);

            builder.HasIndex(c => c.TicketId);

        }
    }
}
