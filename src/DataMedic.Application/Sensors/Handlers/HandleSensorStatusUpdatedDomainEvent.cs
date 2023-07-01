namespace DataMedic.Application.Sensors.Handlers;

public class HandleSensorStatusUpdatedDomainEvent : IHandleSensorStatusUpdatedDomainEvent
{
    public Task Handle(Guid sensorId, bool status)
    {
        Console.WriteLine($"sensor with {sensorId} updated");
        return Task.CompletedTask;
    }
}

public interface IHandleSensorStatusUpdatedDomainEvent
{
    public Task Handle(Guid sensorId, bool status);
}