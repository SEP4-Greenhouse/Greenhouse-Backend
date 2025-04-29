namespace Domain.DTOs
{
    public class PredictionResultDto
    {
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public string Suggestion { get; set; }
        public string TrendAnalysis { get; set; }
    }
}