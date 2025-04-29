public class SensorDataDto
{
    public SensorData current { get; set; }
    public List<SensorData> history { get; set; }
}

public class SensorData
{
    public string sensorType { get; set; }
    public float value { get; set; }
    public DateTime timestamp { get; set; }
}