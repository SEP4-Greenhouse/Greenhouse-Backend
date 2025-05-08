using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Greenhouse
{
    [Key] public int Id { get; private set; }
    [Required] [MaxLength(100)] public string PlantType { get; private set; }
    [ForeignKey("User")] public int UserId { get; private set; }
    public User User { get; private set; }

    public ICollection<Plant> Plants { get; private set; } = new List<Plant>();
    public ICollection<Sensor> Sensors { get; private set; } = new List<Sensor>();
    public ICollection<Controller> Controllers { get; private set; } = new List<Controller>();

    public Greenhouse(string plantType, User user)
    {
        PlantType = plantType;
        User = user;
        UserId = user.Id;
    }

    private Greenhouse()
    {
    } // Required by EF Core

    public void UpdatePlantType(string newPlantType)
    {
        if (string.IsNullOrWhiteSpace(newPlantType))
            throw new ArgumentException("Plant type cannot be empty.");
        PlantType = newPlantType;
    }

    public void AddPlant(Plant plant)
    {
        Plants.Add(plant);
    }

    public void AddSensor(Sensor sensor)
    {
        Sensors.Add(sensor);
    }

    public void AddController(Controller controller)
    {
        Controllers.Add(controller);
    }
}