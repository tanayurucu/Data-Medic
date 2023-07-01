using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.OperatingSystems.Commands.UpdateOperatingSystem;

public sealed record UpdateOperatingSystemCommand(Guid OperatingSystemId, string Name) : ICommand<ErrorOr<Updated>>;