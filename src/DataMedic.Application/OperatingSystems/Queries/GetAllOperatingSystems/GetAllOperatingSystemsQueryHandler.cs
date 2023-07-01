using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.OperatingSystems.Queries.GetAllOperatingSystems;

public sealed class GetAllOperatingSystemsQueryHandler
    : IQueryHandler<GetAllOperatingSystemsQuery, ErrorOr<List<OperatingSystem>>>
{
    private readonly IOperatingSystemRepository _operatingSystemRepository;

    public GetAllOperatingSystemsQueryHandler(IOperatingSystemRepository operatingSystemRepository)
    {
        _operatingSystemRepository = operatingSystemRepository;
    }

    public async Task<ErrorOr<List<OperatingSystem>>> Handle(
        GetAllOperatingSystemsQuery request,
        CancellationToken cancellationToken
    ) => await _operatingSystemRepository.FindAllAsync(cancellationToken);
}