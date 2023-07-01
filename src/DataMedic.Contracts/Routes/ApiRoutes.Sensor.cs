namespace DataMedic.Contracts.Routes;

public static partial class ApiRoutes
{
    public static class Sensors
    {
        public const string Get = "sensors";
        public const string GetTree = "sensors/tree";
        public const string GetById = "sensors/{sensorId:guid}";
        public const string Create = "sensors";
        public const string Update = "sensors/{sensorId:guid}";
        public const string Delete = "sensors/{sensorId:guid}";
    }
}
