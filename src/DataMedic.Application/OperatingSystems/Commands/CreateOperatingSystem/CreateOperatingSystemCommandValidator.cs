using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.OperatingSystems.Commands.CreateOperatingSystem;

public sealed class CreateOperatingSystemCommandValidator : AbstractValidator<CreateOperatingSystemCommand>
{
    public CreateOperatingSystemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.OperatingSystem.Name.Empty)
            .MaximumLength(OperatingSystemName.MaxLength)
            .WithError(Errors.OperatingSystem.Name.TooLong);
    }
}