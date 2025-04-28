namespace Domain.Entities;

public class Controller
{
    public int Id { get; private set; }
    public string Type { get; private set; }
    public string Status { get; private set; }
    public int GreenhouseId { get; private set; }
    public Greenhouse Greenhouse { get; private set; }
    public IReadOnlyCollection<Action> Actions { get; private set; }

    public Controller(string type, string status, Greenhouse greenhouse)
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

    private Controller() { }

    public void UpdateStatus(string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
            throw new ArgumentException("Status cannot be empty.");
        Status = newStatus;
    }

    public Action InitiateAction(DateTime timestamp, string type, double value)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type cannot be empty.");

        var action = new Action(timestamp, type, value, this);
        ((List<Action>)Actions).Add(action);
        return action;
    }
}