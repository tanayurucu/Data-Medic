using DataMedic.Domain.Components.ValueObjects;

using ErrorOr;

namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static class Component
    {
        public static Error IdRequired =>
            Error.Validation(
                code: "Component.IdRequired",
                description: "Component ID is required."
            );
        public static Error NotFound =>
            Error.NotFound(
                code: "Component.NotFound",
                description: "Component with given id not found."
            );

        public static class Name
        {
            public static Error Empty =>
                Error.Validation(
                    code: "Component.Name.Empty",
                    description: "Component name is required."
                );

            public static Error TooLong =>
                Error.Validation(
                    code: "Component.Name.TooLong",
                    description: $"Component name must be at most {ComponentName.MaxLength} characters long."
                );

            public static Func<ComponentName, Error> AlreadyExists =>
                name =>
                    Error.Validation(
                        code: "Component.Name.AlreadyExists",
                        description: $"Component with name '{name.Value}' already exists."
                    );
        }
    }
}