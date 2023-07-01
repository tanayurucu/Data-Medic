using DataMedic.Domain.Departments.ValueObjects;

using ErrorOr;

namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static class Department
    {
        public static class Id
        {
            public static Error Empty =>
                Error.Validation(
                    code: "Department.Id.Empty",
                    description: "Department ID is required.");
        }

        public static Func<DepartmentId, Error> NotFound => id =>
            Error.NotFound(
                code: "Department.NotFound",
                description: $"Department with ID '{id}' not found");

        public static Func<DepartmentName, Error> AlreadyExists => name =>
            Error.Validation(
                code: "Department.AlreadyExists",
                description: $"Department with name '{name.Value}' already exists.");

        public static class Name
        {
            public static Error Empty =>
                Error.Validation(
                    code: "Department.Name.Empty",
                    description: "Department name is required.");

            public static Error TooLong =>
                Error.Validation(
                    code: "Department.Name.TooLong",
                    description: $"Department name must be at most {DepartmentName.MaxLength} characters long.");
        }
    }
}