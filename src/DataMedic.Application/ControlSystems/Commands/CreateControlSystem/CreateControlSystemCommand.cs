using DataMedic.Application.Common.Messages;
using DataMedic.Domain.ControlSystems;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Commands.CreateControlSystem;

public sealed record CreateControlSystemCommand(string Name) : ICommand<ErrorOr<ControlSystem>>;