using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Devices.Commands.CreateDevice;

public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
{
    public CreateDeviceCommandValidator()
    {
        RuleFor(x => x.InventoryNumber).NotEmpty().WithError(Errors.Device.InventoryNumber.Empty);

        RuleFor(x => x.Name).NotEmpty().WithError(Errors.Device.Name.Empty);
    }
}