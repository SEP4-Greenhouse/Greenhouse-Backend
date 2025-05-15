using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

public class Greenhouse
{
    [Key]
    public int Id { get; private set; }

    [Required]
    [MaxLength(100)]
    public string PlantType { get; private set; }

    [Required]
    public int UserId { get; private set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; private set; }

    public ICollection<Plant> Plants { get; private set; } = new List<Plant>();
    public ICollection<Sensor> Sensors { get; private set; } = new List<Sensor>();
    public ICollection<Actuator> Controllers { get; private set; } = new List<Actuator>();

    public Greenhouse(string plantType, User user)
    {
        if (string.IsNullOrWhiteSpace(plantType)) throw new ArgumentException("Plant type cannot be empty.");
        PlantType = plantType ?? throw new ArgumentNullException(nameof(plantType));
        User = user ?? throw new ArgumentNullException(nameof(user));
    }

    private Greenhouse() { }

    public void UpdatePlantType(string newPlantType)
    {
        if (string.IsNullOrWhiteSpace(newPlantType))
            throw new ArgumentException("Plant type cannot be empty.");
        PlantType = newPlantType;
    }

    public void AddPlant(Plant plant) => Plants.Add(plant);
    public void AddSensor(Sensor sensor) => Sensors.Add(sensor);
    public void AddController(Actuator actuator) => Controllers.Add(actuator);
}