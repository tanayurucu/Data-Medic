using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Hosts.Commands.DeleteHost;

public sealed record DeleteHostCommand(Guid HostId) : ICommand<ErrorOr<Deleted>>;
