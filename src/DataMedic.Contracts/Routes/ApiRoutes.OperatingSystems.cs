namespace DataMedic.Contracts.Routes;

public static partial class ApiRoutes
{
    public static class OperatingSystems
    {
        public const string Get = "operating-systems";
        public const string GetAll = "operating-systems/all";
        public const string GetById = "operating-systems/{operatingSystemId:guid}";
        public const string Create = "operating-systems";
        public const string Update = "operating-systems/{operatingSystemId:guid}";
        public const string Delete = "operating-systems/{operatingSystemId:guid}";
    }
}