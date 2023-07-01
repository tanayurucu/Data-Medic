using DataMedic.Application.Hosts.Commands.CreateHost;
using DataMedic.Application.Hosts.Commands.DeleteHost;
using DataMedic.Application.Hosts.Commands.UpdateHost;
using DataMedic.Application.Hosts.Queries.GetAllHosts;
using DataMedic.Application.Hosts.Queries.GetHostById;
using DataMedic.Application.Hosts.Queries.GetHostsWithPagination;
using DataMedic.Application.Hosts.Queries.Kafka.GetKafkaTopics;
using DataMedic.Application.Hosts.Queries.Portainer.GetPortainerContainers;
using DataMedic.Application.Hosts.Queries.Portainer.GetPortainerHosts;
using DataMedic.Contracts.Common;
using DataMedic.Contracts.Hosts;
using DataMedic.Contracts.Hosts.Portainer;
using DataMedic.Contracts.Routes;
using DataMedic.Presentation.Controllers.Base;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataMedic.Presentation.Controllers.V1;

/// <summary>
/// Hosts Controller
/// </summary>
[ApiVersion("1.0")]
public class HostsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates Hosts Controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="mapper"></param>
    public HostsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all hosts
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Hosts.GetAll)]
    [ProducesResponseType(typeof(IReadOnlyCollection<HostResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllHostsQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetAllHostsQuery>(queryParameters);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            hosts => Ok(_mapper.Map<IReadOnlyCollection<HostResponse>>(hosts)),
            Problem
        );
    }

    /// <summary>
    /// Gets Hosts with pagination
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Hosts.GetWithPagination)]
    [ProducesResponseType(typeof(PagedResponse<HostResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWithPagination(
        [FromQuery] GetHostsWithPaginationQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetHostsWithPaginationQuery>(queryParameters);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(value => Ok(_mapper.Map<PagedResponse<HostResponse>>(value)), Problem);
    }

    /// <summary>
    /// Gets a host with given ID
    /// </summary>
    /// <param name="hostId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Hosts.GetById)]
    [ProducesResponseType(typeof(HostResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid hostId,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetHostByIdQuery>(hostId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(value => Ok(_mapper.Map<HostResponse>(value)), Problem);
    }

    /// <summary>
    /// Creates a host
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.Hosts.Create)]
    [ProducesResponseType(typeof(HostResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateHostRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<CreateHostCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            host =>
                CreatedAtAction(
                    actionName: nameof(Create),
                    routeValues: new { HostId = host.Id.Value },
                    value: _mapper.Map<HostResponse>(host)
                ),
            Problem
        );
    }

    /// <summary>
    /// Deletes a host
    /// </summary>
    /// <param name="hostId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.Hosts.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid hostId,
        UpdateHostRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<UpdateHostCommand>((hostId, request));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Deletes a host
    /// </summary>
    /// <param name="hostId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.Hosts.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid hostId,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<DeleteHostCommand>(hostId);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Gets portainer hosts for given host id
    /// </summary>
    /// <param name="hostId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Hosts.GetPortainerHosts)]
    public async Task<IActionResult> GetPortainerHosts(
        Guid hostId,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetPortainerHostsQuery>(hostId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            value => Ok(_mapper.Map<List<PortainerHostInformationResponse>>(value)),
            Problem
        );
    }

    /// <summary>
    /// Gets containers for given host and portainer host
    /// </summary>
    /// <param name="hostId"></param>
    /// <param name="portainerHostId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Hosts.GetPortainerContainers)]
    public async Task<IActionResult> GetPortainerContainers(
        Guid hostId,
        int portainerHostId,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetPortainerContainersQuery>((hostId, portainerHostId));

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            (value) => Ok(_mapper.Map<List<PortainerContainerInformationResponse>>(value)),
            Problem
        );
    }

    /// <summary>
    /// Gets kafka topics for given host
    /// </summary>
    /// <param name="hostId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Hosts.GetKafkaTopics)]
    public async Task<IActionResult> GetKafkaTopics(
        Guid hostId,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetKafkaTopicsQuery>(hostId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(value => Ok(value), Problem);
    }
}
