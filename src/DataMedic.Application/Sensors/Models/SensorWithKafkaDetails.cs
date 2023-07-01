using DataMedic.Domain.Sensors.Entities;

namespace DataMedic.Application.Sensors.Models;

public class SensorWithKafkaDetails
{
    public bool Status { get; set; }
    public string StatusText { get; set; }
    public KafkaSensor SensorDetail { get; set; }
}