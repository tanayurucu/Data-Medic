using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.DeviceGroups.Queries.GetDeviceGroupById;

public sealed class GetDeviceGroupByIdQueryValidator : AbstractValidator<GetDeviceGroupByIdQuery>
{
    public GetDeviceGroupByIdQueryValidator()
    {
        RuleFor(x => x.DeviceGroupId)
            .NotEmpty()
            .WithError(Errors.DeviceGroup.Id.Empty)
            .NotEqual(Guid.Empty)
            .WithError(Errors.DeviceGroup.Id.Empty);
    }
}