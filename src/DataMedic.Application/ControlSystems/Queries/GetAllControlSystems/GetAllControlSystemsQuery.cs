using DataMedic.Application.Common.Messages;
using DataMedic.Domain.ControlSystems;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Queries.GetAllControlSystems;

public sealed record GetAllControlSystemsQuery() : IQuery<ErrorOr<List<ControlSystem>>>;