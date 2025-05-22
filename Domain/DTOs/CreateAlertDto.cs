using Domain.Entities;

namespace Domain.DTOs;

public class CreateAlertDto(string message)
{
    public Alert.AlertType Type { get; set; }
    public string Message { get; set; } = message;
}