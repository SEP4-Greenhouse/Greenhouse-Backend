﻿namespace Domain.DTOs;

public class PredictionResultDto
{
    public DateTime PredictionTime { get; set; }
    public double HoursUntilNextWatering { get; set; }

    public PredictionResultDto()
    {
        
    }
}