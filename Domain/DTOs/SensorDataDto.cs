namespace Domain.DTOs
{
    public class SensorReadingDto
    {
        public string SensorType { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class SensorDataDto
    {
        public SensorReadingDto Current { get; set; }
        public List<SensorReadingDto> History { get; set; }
    }
}