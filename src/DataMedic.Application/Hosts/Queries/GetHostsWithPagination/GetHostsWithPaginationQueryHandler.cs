using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Hosts.Queries.GetHostsWithPagination;

public sealed class GetHostsWithPaginationQueryHandler
    : IQueryHandler<GetHostsWithPaginationQuery, ErrorOr<Paged<Host>>>
{
    private readonly IHostRepository _hostRepository;

    public GetHostsWithPaginationQueryHandler(IHostRepository hostRepository)
    {
        _hostRepository = hostRepository;
    }

    public async Task<ErrorOr<Paged<Host>>> Handle(
        GetHostsWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.HostType is not null && !Enum.IsDefined(typeof(HostType), request.HostType))
        {
            return Errors.Host.InvalidHostType;
        }

        return await _hostRepository.FindManyWithPaginationAndFiltersAsync(
            (HostType?)request.HostType,
            request.SearchString,
            request.PageSize,
            request.PageNumber,
            cancellationToken
        );
    }
}
