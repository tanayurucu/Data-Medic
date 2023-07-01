using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.Sensors.ValueObjects;

using ErrorOr;

namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static class DeviceGroup
    {
        public static Func<DeviceGroupId, Error> NotFound => id =>
            Error.Validation(
                code: "DeviceGroup.NotFound",
                description: $"Device group with id '{id}' not found."
            );

        public static class Id
        {
            public static Error Empty =>
                Error.Validation(
                    code: "DeviceGroup.Id.Empty",
                    description: "Device group ID is required.");
        }

        public static class Name
        {
            public static Error Empty =>
                Error.Validation(
                    code: "DeviceGroup.Name.Empty",
                    description: "Device group name is required."
                );

            public static Error TooLong =>
                Error.Validation(
                    code: "DeviceGroup.DeviceGroupName.TooLong",
                    description:
                    $"Device group name must be at most {DeviceGroupName.MaxLength} characters long."
                );
            public static Func<DeviceGroupName, Error> AlreadyExists => name => Error.Validation(
                code: "DeviceGroup.AlreadyExists",
                description: $"Device group with name '{name.Value}' already exists."
                );
        }
    }
}