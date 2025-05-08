namespace Domain.IServices
{
    using Domain.Entities;

    public interface ISensorDataService
    {
        //Latest reading from all sensors
        Task<IEnumerable<SensorReading>> GetLatestReadingFromAllSensorsAsync();

        //List of readings by sensor
        Task<IDictionary<int, IEnumerable<SensorReading>>> GetReadingsBySensorAsync();

        //All readings from all sensor by timestamp
        Task<IEnumerable<SensorReading>> GetReadingsByTimestampRangeAsync(DateTime start, DateTime end);

        //All readings from all sensor with pagination
        Task<IEnumerable<SensorReading>> GetReadingsPaginatedAsync(int sensorId, int pageNumber, int pageSize);

        //Average reading for a specific sensor
        Task<double> GetAverageReadingForSensorAsync(int sensorId, DateTime start, DateTime end);

        //Latest reading by each sensor
        Task<IDictionary<int, SensorReading>> GetLatestReadingBySensorAsync();

        // Add a new sensor reading
        Task AddSensorReadingAsync(SensorReading reading);
        
        //Trigger alert if threshold exceeded
        Task TriggerAlertIfThresholdExceededAsync(SensorReading reading);
        
        //Get all alerts caused by sensor readings
        Task<IEnumerable<Alert>> GetAllSensorsReadingAlertsAsync();
    }
}