using Microsoft.EntityFrameworkCore;
using CheaperThanAi.Shared.dto;

namespace CheaperThanAi.Server.Data
{
    public class TicketsDbContext : DbContext
    {
        public TicketsDbContext(DbContextOptions<TicketsDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("tickets");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.DateTime).IsRequired();
                entity.Property(e => e.UserName).HasMaxLength(200);
                entity.Property(e => e.IssueDescription);
                entity.Property(e => e.PriorityLevel).HasConversion<int>();
                entity.Property(e => e.Category).HasMaxLength(200);
            });
        }
    }
}
