namespace DataMedic.Contracts.Routes;

public static partial class ApiRoutes
{
    public static class Departments
    {
        public const string GetWithPagination = "departments";
        public const string GetAll = "departments/all";
        public const string GetById = "departments/{departmentId:guid}";
        public const string Create = "departments";
        public const string Update = "departments/{departmentId:guid}";
        public const string Delete = "departments/{departmentId:guid}";
    }
}