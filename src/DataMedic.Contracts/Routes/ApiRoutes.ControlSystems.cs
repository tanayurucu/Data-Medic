namespace DataMedic.Contracts.Routes;

public static partial class ApiRoutes
{
    public static class ControlSystems
    {
        public const string Get = "control-systems";
        public const string GetAll = "control-systems/all";
        public const string GetById = "control-systems/{controlSystemId:guid}";
        public const string Create = "control-systems";
        public const string Update = "control-systems/{controlSystemId:guid}";
        public const string Delete = "control-systems/{controlSystemId:guid}";
    }
}