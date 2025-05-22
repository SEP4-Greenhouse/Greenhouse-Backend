using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Actuators;

namespace Domain.Entities
{
    public class ActuatorAction
    {
        [Key] public int Id { get; private set; }

        [Required] public DateTime Timestamp { get; private set; }

        [Required] [MaxLength(100)] public string Action { get; private set; }

        [Required] public double Value { get; private set; }

        [ForeignKey("Actuator")] public int ActuatorId { get; private set; }
        public Actuator Actuator { get; private set; }

        public List<Alert> TriggeredAlerts { get; private set; }

        public ActuatorAction(DateTime timestamp, string action, double value, Actuator actuator)
        {
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Type cannot be empty.");

            Timestamp = timestamp;
            Action = action;
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