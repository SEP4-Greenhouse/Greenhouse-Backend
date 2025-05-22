namespace Domain.DTOs;

public class GreenhouseDto(string? name, string? plantType, int userId)
{
    public string? Name { get; set; } = name;
    public string? PlantType { get; set; } = plantType;
    public int UserId { get; set; } = userId;
}