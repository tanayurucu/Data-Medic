using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Devices.Queries.GetDeviceById;

public sealed class GetDeviceByIdQueryValidator : AbstractValidator<GetDeviceByIdQuery>
{
    public GetDeviceByIdQueryValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .WithError(Errors.Device.IdRequired)
            .NotEqual(Guid.Empty)
            .WithError(Errors.Device.IdRequired);
    }
}