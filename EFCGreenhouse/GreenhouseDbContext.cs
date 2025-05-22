using Domain.Entities;
using Domain.Entities.Actuators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCGreenhouse;

public class GreenhouseDbContext(DbContextOptions<GreenhouseDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Greenhouse> Greenhouses => Set<Greenhouse>();
    public DbSet<Plant> Plants => Set<Plant>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<SensorReading> SensorReadings => Set<SensorReading>();
    public DbSet<Actuator> Actuators => Set<Actuator>();
    public DbSet<ActuatorAction> ActuatorActions => Set<ActuatorAction>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<Threshold> Thresholds => Set<Threshold>();
    public DbSet<PredictionLog> PredictionLogs => Set<PredictionLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Greenhouses)
            .WithOne(g => g.User)
            .HasForeignKey(g => g.UserId);

        modelBuilder.Entity<Greenhouse>()
            .Property(g => g.Id)
            .HasColumnOrder(0);

        modelBuilder.Entity<Greenhouse>()
            .Property(g => g.Name)
            .HasColumnOrder(1);

        modelBuilder.Entity<Greenhouse>()
            .Property(g => g.PlantType)
            .HasColumnOrder(2);

        modelBuilder.Entity<Greenhouse>()
            .Property(g => g.UserId)
            .HasColumnOrder(3);

        modelBuilder.Entity<Greenhouse>()
            .HasMany(g => g.Plants)
            .WithOne(p => p.Greenhouse)
            .HasForeignKey(p => p.GreenhouseId);

        modelBuilder.Entity<Greenhouse>()
            .HasMany(g => g.Sensors)
            .WithOne(s => s.Greenhouse)
            .HasForeignKey(s => s.GreenhouseId);

        modelBuilder.Entity<Greenhouse>()
            .HasMany(g => g.Actuators)
            .WithOne(a => a.Greenhouse)
            .HasForeignKey(a => a.GreenhouseId);

        modelBuilder.Entity<Sensor>()
            .HasMany(s => s.Readings)
            .WithOne(r => r.Sensor)
            .HasForeignKey(r => r.SensorId);

        modelBuilder.Entity<Actuator>()
            .HasMany(a => a.Actions)
            .WithOne(a => a.Actuator)
            .HasForeignKey(a => a.ActuatorId);

        modelBuilder.Entity<Alert>()
            .HasMany(a => (ICollection<SensorReading>)a.TriggeringSensorReadings)
            .WithMany(r => r.TriggeredAlerts)
            .UsingEntity(
                "AlertSensorReading",
                l => l.HasOne(typeof(SensorReading)).WithMany().HasForeignKey("SensorReadingId"),
                r => r.HasOne(typeof(Alert)).WithMany().HasForeignKey("AlertId"));

        modelBuilder.Entity<Alert>()
            .HasMany(a => (ICollection<ActuatorAction>)a.TriggeringActions)
            .WithMany(a => a.TriggeredAlerts)
            .UsingEntity(
                "AlertActuatorAction",
                l => l.HasOne(typeof(ActuatorAction)).WithMany().HasForeignKey("ActuatorActionId"),
                r => r.HasOne(typeof(Alert)).WithMany().HasForeignKey("AlertId"));

        modelBuilder.Entity<Actuator>()
            .HasDiscriminator<string>("ActuatorType")
            .HasValue<WaterPumpActuator>("WaterPump")
            .HasValue<ServoMotorActuator>("servomotor");

        modelBuilder.Entity<Sensor>()
            .Property(s => s.Id)
            .UseIdentityAlwaysColumn();

        modelBuilder.Entity<Actuator>()
            .Property(s => s.Id)
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd()
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
    }
}