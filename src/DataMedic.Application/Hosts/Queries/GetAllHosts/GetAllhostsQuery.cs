using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Hosts;

using ErrorOr;

namespace DataMedic.Application.Hosts.Queries.GetAllHosts;

public sealed record GetAllHostsQuery(int? HostType) : IQuery<ErrorOr<List<Host>>>;
