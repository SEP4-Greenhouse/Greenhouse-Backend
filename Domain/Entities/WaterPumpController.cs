//Example of a WaterPumpController class that inherits from a base Controller class

namespace Domain.Entities;

public class WaterPumpController : Controller
{
    public WaterPumpController(string status, Greenhouse greenhouse)
        : base("WaterPump", status, greenhouse)
    {
    }

    public Action TurnOn()
    {
        return InitiateAction(DateTime.Now, "TurnOn", 1); // Value could represent flow rate or power usage
    }

    public Action TurnOff()
    {
        return InitiateAction(DateTime.Now, "TurnOff", 0); // Value could represent 0 as the pump is off
    }

    public Action SetFlowRate(double flowRate)
    {
        if (flowRate <= 0)
        {
            throw new ArgumentException("Flow rate must be greater than zero.");
        }

        return InitiateAction(DateTime.Now, "SetFlowRate", flowRate);
    }
}