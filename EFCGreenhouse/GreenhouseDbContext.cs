using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse;

public class GreenhouseDbContext : DbContext
{
    public GreenhouseDbContext(DbContextOptions<GreenhouseDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Greenhouse> Greenhouses => Set<Greenhouse>();
    public DbSet<Plant> Plants => Set<Plant>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<SensorReading> SensorReadings => Set<SensorReading>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<PredictionLog> PredictionLogs => Set<PredictionLog>();
    public DbSet<Controller> Controllers => Set<Controller>();
    public DbSet<WaterPumpController> WaterPumpControllers => Set<WaterPumpController>();
    public DbSet<ControllerAction> ControllerActions => Set<ControllerAction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        

        // Many-to-Many: Plant <-> SensorReading
        modelBuilder.Entity<Plant>()
            .HasMany(p => p.AffectingReadings)
            .WithMany(r => r.AffectedPlants)
            .UsingEntity(
                "PlantSensorReading",
                l => l.HasOne(typeof(SensorReading)).WithMany().HasForeignKey("SensorReadingId"),
                r => r.HasOne(typeof(Plant)).WithMany().HasForeignKey("PlantId"));

        // One-to-Many: User -> Greenhouses
        modelBuilder.Entity<User>()
            .HasMany(u => u.Greenhouses)
            .WithOne(g => g.User)
            .HasForeignKey(g => g.UserId);

        // One-to-Many: Greenhouse -> Plants/Sensors/Controllers
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

        // One-to-Many: Controller -> ControllerActions
        modelBuilder.Entity<Controller>()
            .HasMany(c => c.Actions)
            .WithOne(a => a.Controller)
            .HasForeignKey(a => a.ControllerId);

        // One-to-Many: Sensor -> SensorReadings
        modelBuilder.Entity<Sensor>()
            .HasMany(s => s.Readings)
            .WithOne(r => r.Sensor)
            .HasForeignKey(r => r.SensorId);

        // TPH mapping for controllers
        modelBuilder.Entity<Controller>()
            .HasDiscriminator<string>("ControllerType")
            .HasValue<WaterPumpController>("WaterPump");

        // Controller ID not auto-generated
        modelBuilder.Entity<Controller>()
            .Property(c => c.Id)
            .ValueGeneratedNever();

        // Sensor ID not auto-generated
        modelBuilder.Entity<Sensor>()
            .Property(s => s.Id)
            .ValueGeneratedNever();

        // Many-to-Many: Alert <-> SensorReading
        modelBuilder.Entity<Alert>()
            .HasMany(a => (ICollection<SensorReading>)a.TriggeringSensorReadings)
            .WithMany(sr => sr.TriggeredAlerts)
            .UsingEntity(
                "AlertSensorReading",
                l => l.HasOne(typeof(SensorReading)).WithMany().HasForeignKey("SensorReadingId"),
                r => r.HasOne(typeof(Alert)).WithMany().HasForeignKey("AlertId"));

        // Many-to-Many: Alert <-> ControllerAction
        modelBuilder.Entity<Alert>()
            .HasMany(a => (ICollection<ControllerAction>)a.TriggeringActions)
            .WithMany(ca => ca.TriggeredAlerts)
            .UsingEntity(
                "AlertControllerAction",
                l => l.HasOne(typeof(ControllerAction)).WithMany().HasForeignKey("ControllerActionId"),
                r => r.HasOne(typeof(Alert)).WithMany().HasForeignKey("AlertId"));
    }
}