namespace DataMedic.Contracts.Sensors;

public class GetSensorTreeQueryParameters
{
    public string SearchString { get; set; } = string.Empty;
    public Guid ControlSystemId { get; set; }
    public Guid OperatingSystemId { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid DeviceGroupId { get; set; }
    public bool? ShowOnlyUpSensors { get; set; }
    public bool? ShowOnlyDownSensors { get; set; }
    public bool? ShowOnlyInactiveSensors { get; set; }
}
