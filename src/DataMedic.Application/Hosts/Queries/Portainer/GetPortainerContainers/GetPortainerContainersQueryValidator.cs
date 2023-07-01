using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Hosts.Queries.Portainer.GetPortainerContainers;

public sealed class GetPortainerContainersQueryValidator
    : AbstractValidator<GetPortainerContainersQuery>
{
    public GetPortainerContainersQueryValidator()
    {
        RuleFor(x => x.HostId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithError(Errors.Host.HostIdRequired);

        RuleFor(x => x.PortainerHostId).NotEmpty().GreaterThan(0);
        // TODO: add errors
    }
}
