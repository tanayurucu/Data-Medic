using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.OperatingSystems.Commands.DeleteOperatingSystem;

public class DeleteOperatingSystemCommandValidator : AbstractValidator<DeleteOperatingSystemCommand>
{
    public DeleteOperatingSystemCommandValidator()
    {
        RuleFor(x => x.OperatingSystemId)
            .NotEmpty()
            .WithError(Errors.OperatingSystem.Id.Empty)
            .NotEqual(Guid.Empty)
            .WithError(Errors.OperatingSystem.Id.Empty);
    }
}