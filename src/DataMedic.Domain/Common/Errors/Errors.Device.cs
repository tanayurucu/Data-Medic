using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Devices.ValueObjects;

using ErrorOr;

namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static class Device
    {
        public static Error IdRequired =>
            Error.Validation(code: "Device.IdRequired", description: "Device ID is required.");
        public static Error NotFound =>
            Error.Validation(code: "Device.NotFound", description: "Device Not Found.");

        public static class DeviceComponent
        {
            public static Error NotFound =>
                Error.Validation(
                    code: "Device.DeviceComponent.NotFound",
                    description: "DeviceComponent Not Found."
                );
            public static Error IdAlreadyExists =>
                Error.Validation(
                    code: "Device.DeviceComponent.IdAlreadyExists",
                    description: "DeviceComponentId Already Exists."
                );
            public static Error IdRequired =>
                Error.Validation(
                    code: "Device.DeviceComponent.IdRequired",
                    description: "DeviceComponent ID is required."
                );
        }

        public static class Name
        {
            public static Func<string, Error> AlreadyExists =>
                name =>
                    Error.Validation(
                        code: "Device.AlreadyExists",
                        description: $"Device with name '{name}' already exists."
                    );

            public static Error Empty =>
                Error.Validation(
                    code: "Device.Name.Empty",
                    description: "Device Name is required."
                );

            public static Error TooLong =>
                Error.Validation(
                    code: "Device.Name.TooLong",
                    description: $"Device name must be at most {DeviceName.MaxLength} characters long."
                );
        }

        public static class InventoryNumber
        {
            public static Func<string, Error> AlreadyExists =>
                inventoryNumber =>
                    Error.Validation(
                        code: "Device.InventoryNumber.AlreadyExists",
                        description: $"Inventory Number with name '{inventoryNumber}' already exists."
                    );
            public static Error Empty =>
                Error.Validation(
                    code: "Device.InventoryNumber.Empty",
                    description: "Device inventory number is required."
                );

            public static Error TooLong =>
                Error.Validation(
                    code: "Device.InventoryNumber.TooLong",
                    description: $"Device inventory number must be at most {Devices.ValueObjects.InventoryNumber.MaxLength} characters long."
                );
        }

        public static class IpAddress
        {
            public static Error Empty =>
                Error.Validation(
                    code: "Device.IpAddress.Empty",
                    description: "Device IP Address is required."
                );

            public static Error Invalid =>
                Error.Validation(
                    code: "Device.IpAddress.Invalid",
                    description: "Given IP Address is invalid."
                );

            public static Func<Devices.ValueObjects.IpAddress, Error> AlreadyExists =>
                ipAddress =>
                    Error.Validation(
                        code: "Device.IpAddress.AlreadyExists",
                        description: $"A device component with IP Address '{ipAddress.Value}' already exists."
                    );
        }
    }
}
