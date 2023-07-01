using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Commands.UpdateControlSystem;

public sealed record UpdateControlSystemCommand(Guid ControlSystemId, string Name)
    : ICommand<ErrorOr<Updated>>;