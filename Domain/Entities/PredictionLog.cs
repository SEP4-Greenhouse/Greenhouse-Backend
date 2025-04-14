namespace Domain.Entities;

    public class PredictionLog
    {
        public int Id { get; set; }

        // Sensor input
        public string SensorType { get; set; } = null!;
        public double Value { get; set; }
        public DateTime SensorTimestamp { get; set; }

        // Prediction result
        public string Status { get; set; } = null!;
        public string Suggestion { get; set; } = null!;
        public DateTime PredictionTimestamp { get; set; }
    }
