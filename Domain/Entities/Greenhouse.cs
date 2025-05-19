using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

public class Greenhouse
{
    [Key]
    public int Id { get; private set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; private set; }

    [Required]
    [MaxLength(100)]
    public string PlantType { get; private set; }

    [Required]
    public int UserId { get; private set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; private set; }

    public ICollection<Plant> Plants { get; private set; } = new List<Plant>();
    public ICollection<Sensor> Sensors { get; private set; } = new List<Sensor>();
    public ICollection<Actuator> Actuators { get; private set; } = new List<Actuator>();

    public Greenhouse(string name, string plantType, User user)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(plantType)) throw new ArgumentException("Plant type cannot be empty.");
    
        Name = name ?? throw new ArgumentNullException(nameof(name));
        PlantType = plantType ?? throw new ArgumentNullException(nameof(plantType));
        User = user ?? throw new ArgumentNullException(nameof(user));
    }

    public Greenhouse(string name, string plantType, int userId)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(plantType)) throw new ArgumentException("Plant type cannot be empty.");
    
        Name = name ?? throw new ArgumentNullException(nameof(name));
        PlantType = plantType ?? throw new ArgumentNullException(nameof(plantType));
        UserId = userId;
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
    public void AddActuator(Actuator actuator) => Actuators.Add(actuator);
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty.");
        Name = newName;
    }
}