using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ControllerAction
    {
        [Key] public int Id { get; private set; }

        [Required] public DateTime Timestamp { get; private set; }

        [Required] [MaxLength(100)] public string Type { get; private set; }

        [Required] public double Value { get; private set; }

        [ForeignKey("Controller")] public int ControllerId { get; private set; }
        public Controller Controller { get; private set; }

        public List<Alert> TriggeredAlerts { get; private set; }

        public ControllerAction(DateTime timestamp, string type, double value, Controller controller)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty.");

            Timestamp = timestamp;
            Type = type;
            Value = value;
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            ControllerId = controller.Id;
            TriggeredAlerts = new List<Alert>();
        }

        private ControllerAction()
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