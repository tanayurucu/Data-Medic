using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models.Portainer;

using ErrorOr;

namespace DataMedic.Application.Hosts.Queries.Portainer.GetPortainerContainers;

public sealed record GetPortainerContainersQuery(Guid HostId, int PortainerHostId)
    : IQuery<ErrorOr<List<PortainerContainerInformation>>>;
