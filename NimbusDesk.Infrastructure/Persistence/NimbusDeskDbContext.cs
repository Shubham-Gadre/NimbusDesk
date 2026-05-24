using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NimbusDesk.Domain.Entities;
using NimbusDesk.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace NimbusDesk.Infrastructure.Persistence
{
    /// <summary>
    /// Represents the Entity Framework Core database context for the NimbusDesk application.
    /// Manages the application's database including tickets, comments, ticket history, and user identity data.
    /// </summary>
    public sealed class NimbusDeskDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NimbusDeskDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options containing database configuration.</param>
        public NimbusDeskDbContext(DbContextOptions<NimbusDeskDbContext> options)
            : base(options)
        {
        }

        /// <summary>Gets the DbSet for tickets in the database.</summary>
        public DbSet<Ticket> Tickets => Set<Ticket>();

        /// <summary>
        /// Configures entity models and applies all entity configurations from the current assembly.
        /// </summary>
        /// <param name="builder">The model builder used to configure entity models.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(NimbusDeskDbContext).Assembly);
        }
    }
}
