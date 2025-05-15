using Domain.Entities;

namespace Domain.DTOs;

public class CreateAlertDto
{
    public Alert.AlertType Type { get; set; }
    public string Message { get; set; }
}