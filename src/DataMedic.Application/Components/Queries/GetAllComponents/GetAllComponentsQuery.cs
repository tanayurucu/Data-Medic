using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Components;

using ErrorOr;

namespace DataMedic.Application.Components.Queries.GetAllComponents;

public sealed record GetAllComponentsQuery() : IQuery<ErrorOr<List<Component>>>;