using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Components;

using ErrorOr;

namespace DataMedic.Application.Components.Commands.CreateComponent;

public sealed record CreateComponentCommand(string Name) : ICommand<ErrorOr<Component>>;