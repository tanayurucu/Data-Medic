using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Components.Commands.UpdateComponent;

public sealed record UpdateComponentCommand(Guid ComponentId, string Name)
    : ICommand<ErrorOr<Updated>>;