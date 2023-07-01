using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models.Portainer;

using ErrorOr;

namespace DataMedic.Application.Hosts.Queries.Portainer.GetPortainerHosts;

public sealed record GetPortainerHostsQuery(Guid HostId)
    : IQuery<ErrorOr<List<PortainerHostInformation>>>;
