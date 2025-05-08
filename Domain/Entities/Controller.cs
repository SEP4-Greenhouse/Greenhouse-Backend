using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public abstract class Controller
    {
        [Key] public int Id { get; private set; }

        [Required] [MaxLength(100)] public string Type { get; private set; }

        [Required] [MaxLength(100)] public string Status { get; private set; }

        [ForeignKey("Greenhouse")] public int GreenhouseId { get; private set; }
        public Greenhouse Greenhouse { get; private set; }

        public IReadOnlyCollection<Action> Actions { get; private set; }

        protected Controller(string type, string status, Greenhouse greenhouse)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty.");
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be empty.");

            Type = type;
            Status = status;
            Greenhouse = greenhouse ?? throw new ArgumentNullException(nameof(greenhouse));
            GreenhouseId = greenhouse.Id;
            Actions = new List<Action>();
        }

        private Controller()
        {
        }

        public void UpdateStatus(string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
                throw new ArgumentException("Status cannot be empty.");
            Status = newStatus;
        }

        public Action InitiateAction(DateTime timestamp, string type, double value)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Action type cannot be empty.");

            var action = new Action(timestamp, type, value, this);
            ((List<Action>)Actions).Add(action);
            return action;
        }
    }
}