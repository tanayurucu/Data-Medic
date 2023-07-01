namespace DataMedic.Contracts.Routes;

public static partial class ApiRoutes
{
    public static class Components
    {
        public const string Get = "components";
        public const string GetAll = "components/all";
        public const string GetById = "components/{componentId:guid}";
        public const string Create = "components";
        public const string Update = "components/{componentId:guid}";
        public const string Delete = "components/{componentId:guid}";
    }
}
