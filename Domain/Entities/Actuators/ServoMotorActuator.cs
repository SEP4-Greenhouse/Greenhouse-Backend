namespace Domain.Entities.Actuators;

public class ServoMotorActuator : Actuator
{
    public ServoMotorActuator(string status, Greenhouse greenhouse)
        : base(status, greenhouse)
    {
    }
    
    private ServoMotorActuator() : base()
    {
    }
    //TODO: Implement the ServoMotorActuators class
    
}