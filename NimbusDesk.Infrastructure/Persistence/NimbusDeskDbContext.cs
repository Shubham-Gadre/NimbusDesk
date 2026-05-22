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
    public sealed class NimbusDeskDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public NimbusDeskDbContext(DbContextOptions<NimbusDeskDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets => Set<Ticket>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(NimbusDeskDbContext).Assembly);
        }
    }
}
