using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ActuatorAction
    {
        [Key] public int Id { get; private set; }

        [Required] public DateTime Timestamp { get; private set; }

        [Required] [MaxLength(100)] public string Type { get; private set; }

        [Required] public double Value { get; private set; }

        [ForeignKey("Actuator")] public int ActuatorId { get; private set; }
        public Actuator Actuator { get; private set; }

        public List<Alert> TriggeredAlerts { get; private set; }

        public ActuatorAction(DateTime timestamp, string type, double value, Actuator actuator)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty.");

            Timestamp = timestamp;
            Type = type;
            Value = value;
            Actuator = actuator ?? throw new ArgumentNullException(nameof(actuator));
            ActuatorId = actuator.Id;
            TriggeredAlerts = new List<Alert>();
        }

        private ActuatorAction()
        {
        }

        public Alert TriggerAlert(Alert.AlertType alertType, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty.");

            var alert = new Alert(alertType, message);
            TriggeredAlerts.Add(alert);
            return alert;
        }
    }
}