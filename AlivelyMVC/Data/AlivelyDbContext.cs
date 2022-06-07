using AlivelyMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AlivelyMVC.Data
{
    public class AlivelyDbContext : DbContext
    {
        public DbSet<SMARTGoal> SMARTGoals { get; set; }

        public AlivelyDbContext(DbContextOptions<AlivelyDbContext> options)
            : base(options) 
        {

        }

        public DbSet<AlivelyMVC.Models.Task>? Task { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SMARTGoal>()
                .HasMany(g => g.Tasks)
                .WithOne(t => t.SMARTGoal);
        }
    }
}
