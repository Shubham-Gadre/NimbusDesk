using Microsoft.EntityFrameworkCore;
using NimbusDesk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Infrastructure.Persistence
{
    public sealed class NimbusDeskDbContext : DbContext
    {
        public NimbusDeskDbContext(DbContextOptions<NimbusDeskDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets => Set<Ticket>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NimbusDeskDbContext).Assembly);
        }
    }
}
