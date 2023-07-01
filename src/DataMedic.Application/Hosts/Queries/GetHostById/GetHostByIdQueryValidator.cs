using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Hosts.Queries.GetHostById;

public sealed class GetHostByIdQueryValidator : AbstractValidator<GetHostByIdQuery>
{
    public GetHostByIdQueryValidator()
    {
        RuleFor(x => x.HostId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithError(Errors.Host.HostIdRequired);
    }
}
