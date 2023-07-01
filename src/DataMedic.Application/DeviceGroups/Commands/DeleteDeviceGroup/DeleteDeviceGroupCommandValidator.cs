using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.DeviceGroups.Commands.DeleteDeviceGroup;

public class DeleteDeviceGroupCommandValidator : AbstractValidator<DeleteDeviceGroupCommand>
{
    public DeleteDeviceGroupCommandValidator()
    {
        RuleFor(x => x.DeviceGroupId)
            .NotEmpty()
            .WithError(Errors.DeviceGroup.Id.Empty)
            .NotEqual(Guid.Empty)
            .WithError(Errors.DeviceGroup.Id.Empty);
    }
}