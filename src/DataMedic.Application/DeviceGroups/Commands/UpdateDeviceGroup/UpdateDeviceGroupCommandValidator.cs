using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.DeviceGroups.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.DeviceGroups.Commands.UpdateDeviceGroup;

public sealed class UpdateDeviceGroupCommandValidator : AbstractValidator<UpdateDeviceGroupCommand>
{
    public UpdateDeviceGroupCommandValidator()
    {
        RuleFor(x => x.DeviceGroupId)
            .NotEmpty()
            .WithError(Errors.DeviceGroup.Id.Empty)
            .NotEqual(Guid.Empty)
            .WithError(Errors.DeviceGroup.Id.Empty);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.DeviceGroup.Name.Empty)
            .MaximumLength(DeviceGroupName.MaxLength)
            .WithError(Errors.DeviceGroup.Name.TooLong);
    }
}