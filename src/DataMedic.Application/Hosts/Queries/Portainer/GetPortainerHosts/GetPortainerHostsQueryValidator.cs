using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Hosts.Queries.Portainer.GetPortainerHosts;

public sealed class GetPortainerHostsQueryValidator : AbstractValidator<GetPortainerHostsQuery>
{
    public GetPortainerHostsQueryValidator()
    {
        RuleFor(x => x.HostId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithError(Errors.Host.HostIdRequired);
    }
}
