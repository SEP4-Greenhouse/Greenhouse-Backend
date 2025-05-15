using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public abstract class Actuator
    {
        [Key]
        public int Id { get; set; }

        [Required] [MaxLength(100)] public string Type { get; private set; }

        [Required] [MaxLength(100)] public string Status { get; private set; }

        [ForeignKey("Greenhouse")] public int GreenhouseId { get; private set; }

        public Greenhouse Greenhouse { get; private set; }

        public List<ActuatorAction> Actions { get; private set; }

        // Parameterless constructor for EF Core
        protected Actuator()
        {
            Type = string.Empty;
            Status = string.Empty;
            Actions = new List<ActuatorAction>();
        }

        protected Actuator(int id, string type, string status, Greenhouse greenhouse)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be greater than zero.");
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty.");
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be empty.");

            Id = id;
            Type = type;
            Status = status;
            Greenhouse = greenhouse ?? throw new ArgumentNullException(nameof(greenhouse));
            GreenhouseId = greenhouse.Id;
            Actions = new List<ActuatorAction>();
        }

        public void UpdateStatus(string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
                throw new ArgumentException("Status cannot be empty.");
            Status = newStatus;
        }

        public ActuatorAction InitiateAction(DateTime timestamp, string type, double value)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("ControllerControllerAction type cannot be empty.");

            var action = new ActuatorAction(timestamp, type, value, this);
            ((List<ActuatorAction>)Actions).Add(action);
            return action;
        }
    }
}