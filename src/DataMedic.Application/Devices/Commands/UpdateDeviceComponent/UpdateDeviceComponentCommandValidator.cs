using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.Devices.Commands.UpdateDeviceComponent;

internal sealed class UpdateDeviceComponentCommandValidator
    : AbstractValidator<UpdateDeviceComponentCommand>
{
    public UpdateDeviceComponentCommandValidator() { }
}
