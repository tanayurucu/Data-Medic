using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.OperatingSystems.Commands.UpdateOperatingSystem;

public sealed class UpdateOperatingSystemCommandValidator : AbstractValidator<UpdateOperatingSystemCommand>
{
    public UpdateOperatingSystemCommandValidator()
    {
        RuleFor(x => x.OperatingSystemId)
            .NotEmpty()
            .WithError(Errors.OperatingSystem.Id.Empty)
            .NotEqual(Guid.Empty)
            .WithError(Errors.OperatingSystem.Id.Empty);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.OperatingSystem.Name.Empty)
            .MaximumLength(OperatingSystemName.MaxLength)
            .WithError(Errors.OperatingSystem.Name.TooLong);
    }
}