using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse
{
    public class GreenhouseDbContext : DbContext
    {
        public GreenhouseDbContext(DbContextOptions<GreenhouseDbContext> options)
            : base(options) {}

        public DbSet<PredictionLog> PredictionLogs { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }
    }
}