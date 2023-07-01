using DataMedic.Domain.Sensors.ValueObjects;

using ErrorOr;
namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static class Sensor
    {
        public static Error Invalid =>
            Error.Validation(
                code: "Sensor.Invalid",
                description: "Sensor Type is Invalid."
            );

        public static class Id
        {
            public static Error Empty =>
                Error.Validation(
                    code: "Sensor.Id.Empty",
                    description: "Sensor ID is required.");
        }

        public static Func<SensorId, Error> NotFound => id =>
            Error.NotFound(
                code: "Sensor.NotFound",
                description: $"Sensor with ID '{id}' not found");
    }
}