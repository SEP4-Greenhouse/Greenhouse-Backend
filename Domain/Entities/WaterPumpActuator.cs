namespace Domain.Entities;

public class WaterPumpActuator : Actuator
{
    // Parameterless constructor required by EF Core
    private WaterPumpActuator() : base()
    {
    }


    public WaterPumpActuator(string status, Greenhouse greenhouse)
        : base(status, greenhouse)
    {
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