using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.ControlSystems.Queries.GetControlSystemById;

public sealed class GetControlSystemByIdQueryValidator : AbstractValidator<GetControlSystemByIdQuery>
{
    public GetControlSystemByIdQueryValidator()
    {
        RuleFor(x => x.ControlSystemId)
            .NotEmpty()
            .WithError(Errors.ControlSystem.IdRequired)
            .NotEqual(Guid.Empty)
            .WithError(Errors.ControlSystem.IdRequired);
    }
}