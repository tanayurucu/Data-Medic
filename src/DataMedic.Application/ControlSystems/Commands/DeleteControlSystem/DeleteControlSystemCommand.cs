using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Commands.DeleteControlSystem;

public sealed record DeleteControlSystemCommand(Guid ControlSystemId) : ICommand<ErrorOr<Deleted>>;