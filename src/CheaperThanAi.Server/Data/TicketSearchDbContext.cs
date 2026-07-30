using Microsoft.EntityFrameworkCore;
using CheaperThanAi.Shared.dto;

namespace CheaperThanAi.Server.Data
{
    public class TicketSearchDbContext : DbContext
    {
        public TicketSearchDbContext(DbContextOptions<TicketSearchDbContext> options) : base(options)
        {
        }

        public DbSet<TicketSearchResult> TicketSearchResult { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketSearchResult>(entity =>
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
