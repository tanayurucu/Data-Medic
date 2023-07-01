using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Devices.Queries.GetAllDeviceComponents;

public sealed class GetAllDeviceComponentsQueryValidator
    : AbstractValidator<GetAllDeviceComponentsQuery>
{
    public GetAllDeviceComponentsQueryValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithError(Errors.Device.IdRequired);
    }
}
