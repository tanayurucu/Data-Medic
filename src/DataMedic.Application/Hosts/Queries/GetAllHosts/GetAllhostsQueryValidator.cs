using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.Hosts.Queries.GetAllHosts;

public sealed class GetAllHostsQueryValidator : AbstractValidator<GetAllHostsQuery>
{
    public GetAllHostsQueryValidator()
    {
        RuleFor(x => x.HostType)
            .Must(value => value is null || Enum.IsDefined(typeof(HostType), value))
            .WithError(Errors.Host.InvalidHostType);
    }
}
