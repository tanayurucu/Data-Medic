using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.DeviceGroups.ValueObjects;

using FluentValidation;
namespace DataMedic.Application.DeviceGroups.Commands.CreateDeviceGroup;

public sealed class CreateDeviceGroupCommandValidator : AbstractValidator<CreateDeviceGroupCommand>
{
    public CreateDeviceGroupCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.DeviceGroup.Name.Empty)
            .MaximumLength(DeviceGroupName.MaxLength)
            .WithError(Errors.DeviceGroup.Name.TooLong);
    }
}