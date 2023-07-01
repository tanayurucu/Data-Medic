using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.OperatingSystems.Commands.DeleteOperatingSystem;

public sealed record DeleteOperatingSystemCommand(Guid OperatingSystemId) : ICommand<ErrorOr<Deleted>>;