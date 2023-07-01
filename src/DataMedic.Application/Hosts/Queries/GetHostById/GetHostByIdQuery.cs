using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Hosts;

using ErrorOr;

namespace DataMedic.Application.Hosts.Queries.GetHostById;

public sealed record GetHostByIdQuery(Guid HostId) : IQuery<ErrorOr<Host>>;
