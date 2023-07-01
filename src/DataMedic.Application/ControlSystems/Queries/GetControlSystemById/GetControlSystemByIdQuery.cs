using DataMedic.Application.Common.Messages;
using DataMedic.Domain.ControlSystems;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Queries.GetControlSystemById;

public sealed record GetControlSystemByIdQuery(Guid ControlSystemId) : IQuery<ErrorOr<ControlSystem>>;