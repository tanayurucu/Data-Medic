using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.ControlSystems;

using ErrorOr;

namespace DataMedic.Application.ControlSystems.Queries.GetAllControlSystems;

public sealed class GetAllControlSystemsQueryHandler
    : IQueryHandler<GetAllControlSystemsQuery, ErrorOr<List<ControlSystem>>>
{
    private readonly IControlSystemRepository _controlSystemRepository;

    public GetAllControlSystemsQueryHandler(IControlSystemRepository controlSystemRepository)
    {
        _controlSystemRepository = controlSystemRepository;
    }

    public async Task<ErrorOr<List<ControlSystem>>> Handle(
        GetAllControlSystemsQuery request,
        CancellationToken cancellationToken
    ) => await _controlSystemRepository.FindAllAsync(cancellationToken);
}