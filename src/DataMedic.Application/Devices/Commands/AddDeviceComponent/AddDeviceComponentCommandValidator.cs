using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.Devices.Commands.AddDeviceComponent;

public sealed class AddDeviceCommandValidator : AbstractValidator<AddDeviceComponentCommand>
{
    public AddDeviceCommandValidator()
    {
        RuleFor(x => x.IpAddress).NotEmpty().WithError(Errors.Device.IpAddress.Empty);
    }
}
