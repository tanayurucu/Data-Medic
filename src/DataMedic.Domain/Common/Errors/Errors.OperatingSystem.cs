using DataMedic.Domain.OperatingSystems.ValueObjects;

using ErrorOr;

namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static class OperatingSystem
    {
        public static Error NotFound =>
            Error.NotFound(
                code: "OperatingSystem.NotFound",
                description: "Operating system with given ID not found."
            );

        public static class Id
        {
            public static Error Empty =>
                Error.Validation(
                    code: "OperatingSystem.Id.Empty",
                    description: "Operating system ID is required."
                );
        }

        public static class Name
        {
            public static Error Empty =>
                Error.Validation(
                    code: "OperatingSystem.Name.Empty",
                    description: "Operating system name is required."
                );

            public static Error TooLong =>
                Error.Validation(
                    code: "OperatingSystem.Name.TooLong",
                    description: $"Operating system name must be at most {OperatingSystemName.MaxLength} characters long."
                );

            public static Func<OperatingSystemName, Error> AlreadyExists =>
                name =>
                    Error.Conflict(
                        code: "OperatingSystem.Name.AlreadyExists",
                        description: $"Operating system with name '{name.Value}' already exists."
                    );
        }
    }
}
