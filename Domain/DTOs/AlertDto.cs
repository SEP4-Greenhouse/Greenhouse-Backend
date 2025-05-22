using System;

namespace Domain.DTOs;

public class AlertDto(string message)
{
    public string Type { get; set; }
    public string Message { get; set; } = message;
    public DateTime Timestamp { get; set; }
}