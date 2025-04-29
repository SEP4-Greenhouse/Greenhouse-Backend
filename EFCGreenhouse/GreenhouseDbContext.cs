using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse
{
    public class GreenhouseDbContext : DbContext
    {
        public GreenhouseDbContext(DbContextOptions<GreenhouseDbContext> options)
            : base(options) {}

        public DbSet<PredictionLog> PredictionLogs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the primary key for PredictionLog
            modelBuilder.Entity<PredictionLog>().HasKey(p => p.Id); // Assuming 'Id' is your primary key
            
            base.OnModelCreating(modelBuilder);
        }
    }
}