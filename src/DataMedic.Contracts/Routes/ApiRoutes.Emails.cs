namespace DataMedic.Contracts.Routes;

public static partial class ApiRoutes
{
    public static class Emails
    {
        public const string Get = "emails";
        public const string GetById = "emails/{emailId:guid}";
        public const string Create = "emails";
        public const string Update = "emails/{emailId:guid}";
        public const string Delete = "emails/{emailId:guid}";
    }
}