using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Worker.Jobs.KafkaScanner.Models;

public class SensorWithKafkaDetails
{
    public SensorId SensorId { get; set; }
    public bool Status { get; set; }
    public string StatusText { get; set; }
    public KafkaSensor SensorDetail { get; set; }
}