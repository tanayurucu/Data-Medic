using DataMedic.Application.ControlSystems.Queries.GetAllControlSystems;
using DataMedic.Application.OperatingSystems.Commands.CreateOperatingSystem;
using DataMedic.Application.OperatingSystems.Commands.DeleteOperatingSystem;
using DataMedic.Application.OperatingSystems.Commands.UpdateOperatingSystem;
using DataMedic.Application.OperatingSystems.Queries.GetAllOperatingSystems;
using DataMedic.Application.OperatingSystems.Queries.GetOperatingSystemById;
using DataMedic.Application.OperatingSystems.Queries.GetOperatingSystemsWithPagination;
using DataMedic.Contracts.Common;
using DataMedic.Contracts.OperatingSystems;
using DataMedic.Contracts.Routes;
using DataMedic.Infrastructure.Authentication.Constants;
using DataMedic.Presentation.Controllers.Base;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataMedic.Presentation.Controllers.V1;

/// <summary>
/// OperatingSystems Controller
/// </summary>
[ApiVersion("1.0")]
public class OperatingSystemsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates OperatingSystems Controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="mapper"></param>
    public OperatingSystemsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets paginated list of operating systems
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.OperatingSystems.Get)]
    //[Authorize(Policy = PolicyNames.RequireExpertPolicy)]
    [ProducesResponseType(typeof(PagedResponse<OperatingSystemResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] GetOperatingSystemsQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetOperatingSystemsWithPaginationQuery>(queryParameters);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            operatingSystems =>
                Ok(_mapper.Map<PagedResponse<OperatingSystemResponse>>(operatingSystems)),
            Problem
        );
    }

    /// <summary>
    /// Gets all operating systems
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.OperatingSystems.GetAll)]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<OperatingSystemResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var query = new GetAllOperatingSystemsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            value => Ok(_mapper.Map<IReadOnlyCollection<OperatingSystemResponse>>(value)),
            Problem
        );
    }

    /// <summary>
    /// Gets a operating system by ID
    /// </summary>
    /// <param name="operatingSystemId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.OperatingSystems.GetById)]
    [ProducesResponseType(typeof(OperatingSystemResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid operatingSystemId,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetOperatingSystemByIdQuery>(operatingSystemId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            operatingSystem => Ok(_mapper.Map<OperatingSystemResponse>(operatingSystem)),
            Problem
        );
    }

    /// <summary>
    /// Creates a operating system
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.OperatingSystems.Create)]
    [ProducesResponseType(typeof(OperatingSystemResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateOperatingSystemRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<CreateOperatingSystemCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            operatingSystem =>
                CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { OperatingSystemId = operatingSystem.Id.Value },
                    value: _mapper.Map<OperatingSystemResponse>(operatingSystem)
                ),
            Problem
        );
    }

    /// <summary>
    /// Updates a operating system
    /// </summary>
    /// <param name="operatingSystemId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.OperatingSystems.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid operatingSystemId,
        UpdateOperatingSystemRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<UpdateOperatingSystemCommand>((operatingSystemId, request));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Deletes a operating system
    /// </summary>
    /// <param name="operatingSystemId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.OperatingSystems.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid operatingSystemId,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<DeleteOperatingSystemCommand>(operatingSystemId);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}