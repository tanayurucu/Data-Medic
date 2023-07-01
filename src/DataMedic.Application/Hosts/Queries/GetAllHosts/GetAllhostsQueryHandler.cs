using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Hosts.Queries.GetAllHosts;

public sealed class GetAllHostsQueryHandler : IQueryHandler<GetAllHostsQuery, ErrorOr<List<Host>>>
{
    private readonly IHostRepository _hostRepository;

    public GetAllHostsQueryHandler(IHostRepository hostRepository)
    {
        _hostRepository = hostRepository;
    }

    public async Task<ErrorOr<List<Host>>> Handle(
        GetAllHostsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.HostType is not null && !Enum.IsDefined(typeof(HostType), request.HostType))
        {
            return Errors.Host.InvalidHostType;
        }

        return await _hostRepository.FindAllByHostTypeAsync(
            (HostType?)request.HostType,
            cancellationToken
        );
    }
}
