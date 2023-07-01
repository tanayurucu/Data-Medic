namespace DataMedic.Contracts.Routes;

public static partial class ApiRoutes
{
    public static class Devices
    {
        public const string Get = "devices";
        public const string GetAll = "devices/all";
        public const string GetById = "devices/{deviceId:guid}";
        public const string Create = "devices";
        public const string Update = "devices/{deviceId:guid}";
        public const string Delete = "devices/{deviceId:guid}";
        public const string GetComponents = "devices/{deviceId:guid}/components";
        public const string GetAllComponents = "devices/{deviceId:guid}/components/all";
        public const string UpdateDeviceComponent =
            "devices/{deviceId:guid}/components/{deviceComponentId:guid}";
        public const string AddDeviceComponent = "devices/{deviceId:guid}/components";
        public const string DeleteDeviceComponent =
            "devices/{deviceId:guid}/components/{deviceComponentId:guid}";
    }
}
