using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Components;

using ErrorOr;

namespace DataMedic.Application.Components.Commands.DeleteComponent;

public sealed record DeleteComponentCommand(Guid ComponentId) : ICommand<ErrorOr<Component>>;