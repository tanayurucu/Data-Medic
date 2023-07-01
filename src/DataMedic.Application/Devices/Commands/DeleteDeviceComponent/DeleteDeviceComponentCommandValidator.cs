using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Devices.Commands.DeleteDeviceComponent;

public class DeleteDeviceComponentCommandValidator : AbstractValidator<DeleteDeviceComponentCommand>
{
    public DeleteDeviceComponentCommandValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty().WithError(Errors.Device.IdRequired);

        RuleFor(x => x.DeviceComponentId)
            .NotEmpty()
            .WithError(Errors.Device.DeviceComponent.IdRequired);
    }
}
