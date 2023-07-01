using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Worker.Jobs.MqttScanner.Models;

public class SensorWithMqttDetails
{
    public SensorId SensorId { get; set; }
    public bool Status { get; set; }
    public string StatusText { get; set; }
    public MqttSensor SensorDetail { get; set; }
}