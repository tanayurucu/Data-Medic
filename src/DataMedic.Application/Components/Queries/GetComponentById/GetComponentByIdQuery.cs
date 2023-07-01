using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Components;

using ErrorOr;

namespace DataMedic.Application.Components.Queries.GetComponentById;

public sealed record GetComponentByIdQuery(Guid ComponentId) : IQuery<ErrorOr<Component>>;