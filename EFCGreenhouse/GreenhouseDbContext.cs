using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Action = Domain.Entities.Action;

namespace EFCGreenhouse
{
    public class GreenhouseDbContext : DbContext
    {
        public GreenhouseDbContext(DbContextOptions<GreenhouseDbContext> options)
            : base(options) {}

        public DbSet<PredictionLog> PredictionLogs { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Greenhouse> Greenhouses { get; set; }
        public DbSet<Controller> Controllers { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Action>()
                .HasOne(a => a.Controller)
                .WithMany(c => c.Actions)
                .HasForeignKey(a => a.ControllerId);

            modelBuilder.Entity<Alert>()
                .HasMany(a => a.TriggeringSensorReadings)
                .WithMany(sr => sr.TriggeredAlerts);

            modelBuilder.Entity<Alert>()
                .HasMany(a => a.TriggeringActions)
                .WithMany(ac => ac.TriggeredAlerts);

            modelBuilder.Entity<Greenhouse>()
                .HasMany(g => g.Plants)
                .WithOne(p => p.Greenhouse)
                .HasForeignKey(p => p.GreenhouseId);

            modelBuilder.Entity<Greenhouse>()
                .HasMany(g => g.Sensors)
                .WithOne(s => s.Greenhouse)
                .HasForeignKey(s => s.GreenhouseId);

            modelBuilder.Entity<Greenhouse>()
                .HasMany(g => g.Controllers)
                .WithOne(c => c.Greenhouse)
                .HasForeignKey(c => c.GreenhouseId);

            modelBuilder.Entity<SensorReading>()
                .HasOne(sr => sr.Sensor)
                .WithMany(s => s.Readings)
                .HasForeignKey(sr => sr.SensorId);

            modelBuilder.Entity<SensorReading>()
                .HasMany(sr => sr.TriggeredAlerts)
                .WithMany(a => a.TriggeringSensorReadings);

            modelBuilder.Entity<SensorReading>()
                .HasMany(sr => sr.AffectedPlants)
                .WithMany(p => p.AffectingReadings);

            modelBuilder.Entity<Controller>()
                .HasOne(c => c.Greenhouse)
                .WithMany(g => g.Controllers)
                .HasForeignKey(c => c.GreenhouseId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Greenhouses)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId);
            modelBuilder.Entity<Alert>()
                .Property(a => a.Type)
                .HasConversion<string>();


            base.OnModelCreating(modelBuilder);
        }
    }
}
