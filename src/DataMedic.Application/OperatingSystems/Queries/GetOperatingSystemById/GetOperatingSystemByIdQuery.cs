using DataMedic.Application.Common.Messages;
using DataMedic.Domain.OperatingSystems;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.OperatingSystems.Queries.GetOperatingSystemById;

public sealed record GetOperatingSystemByIdQuery(Guid OperatingSystemId) : IQuery<ErrorOr<OperatingSystem>>;