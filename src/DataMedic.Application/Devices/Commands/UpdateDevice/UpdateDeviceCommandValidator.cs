using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices.ValueObjects;

using ErrorOr;

using FluentValidation;

namespace DataMedic.Application.Devices.Commands.UpdateDevice;

public sealed class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(Errors.Device.Name.Empty);

        RuleFor(x => x.InventoryNumber)
            .NotEmpty()
            .WithError(Errors.Device.InventoryNumber.Empty)
            .MaximumLength(InventoryNumber.MaxLength)
            .WithError(Errors.Device.InventoryNumber.TooLong);
    }
}
