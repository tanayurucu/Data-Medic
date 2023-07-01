using DataMedic.Application.Common.Messages;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.OperatingSystems.Queries.GetAllOperatingSystems;

public sealed record GetAllOperatingSystemsQuery() : IQuery<ErrorOr<List<OperatingSystem>>>;