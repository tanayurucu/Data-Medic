namespace DataMedic.Contracts.Routes;

public static partial class ApiRoutes
{
    public static class DeviceGroups
    {
        public const string Get = "device-groups";
        public const string GetAll = "device-groups/all";
        public const string GetById = "device-groups/{deviceGroupId:guid}";
        public const string Create = "device-groups";
        public const string Update = "device-groups/{deviceGroupId:guid}";
        public const string Delete = "device-groups/{deviceGroupId:guid}";
    }
}