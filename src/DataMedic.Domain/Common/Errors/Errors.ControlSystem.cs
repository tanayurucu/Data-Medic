using DataMedic.Domain.ControlSystems.ValueObjects;

using ErrorOr;

namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static class ControlSystem
    {
        public static Error IdRequired =>
            Error.Validation(
                code: "ControlSystem.IdRequired",
                description: "Control system ID is required."
            );
        public static Error NotFound =>
            Error.Validation(
                code: "ControlSystem.NotFound",
                description: "Control system not found."
            );

        public static class Name
        {
            public static Error Empty =>
                Error.Validation(
                    code: "ControlSystem.ControlSystemName.Empty",
                    description: "Control system name is required."
                );

            public static Error TooLong =>
                Error.Validation(
                    code: "ControlSystem.ControlSystemName.TooLong",
                    description: $"Control system name must be at most {ControlSystems.ValueObjects.ControlSystemName.MaxLength} characters long."
                );
            public static Func<ControlSystemName, Error> AlreadyExists =>
                name =>
                    Error.Validation(
                        code: "ControlSystem.ControlSystemName.AlreadyExists",
                        description: $"Control system with name '{name.Value}' already exists."
                    );
        }
    }
}