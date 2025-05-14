namespace Domain.Entities;

public class WaterPumpActuator : Actuator
{
    // Parameterless constructor required by EF Core
    private WaterPumpActuator() : base()
    {
    }


    public WaterPumpActuator(int id, string status, Greenhouse greenhouse)
        : base(id, "WaterPump", status, greenhouse)
    {
    }

    public ActuatorAction TurnOn()
    {
        return InitiateAction(DateTime.Now, "TurnOn", 1); // Value could represent flow rate or power usage
    }

    public ActuatorAction TurnOff()
    {
        return InitiateAction(DateTime.Now, "TurnOff", 0); // Value could represent 0 as the pump is off
    }

    public ActuatorAction SetFlowRate(double flowRate)
    {
        if (flowRate <= 0)
        {
            throw new ArgumentException("Flow rate must be greater than zero.");
        }

        return InitiateAction(DateTime.Now, "SetFlowRate", flowRate);
    }
}