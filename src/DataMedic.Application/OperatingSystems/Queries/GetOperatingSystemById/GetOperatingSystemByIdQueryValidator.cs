using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.OperatingSystems.Queries.GetOperatingSystemById;

public sealed class GetOperatingSystemByIdQueryValidator : AbstractValidator<GetOperatingSystemByIdQuery>
{
    public GetOperatingSystemByIdQueryValidator()
    {
        RuleFor(x => x.OperatingSystemId)
            .NotEmpty()
            .WithError(Errors.OperatingSystem.Id.Empty)
            .NotEqual(Guid.Empty)
            .WithError(Errors.OperatingSystem.Id.Empty);
    }
}