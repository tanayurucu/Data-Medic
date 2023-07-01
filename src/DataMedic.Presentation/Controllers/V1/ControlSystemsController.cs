using DataMedic.Application.ControlSystems.Commands.CreateControlSystem;
using DataMedic.Application.ControlSystems.Commands.DeleteControlSystem;
using DataMedic.Application.ControlSystems.Commands.UpdateControlSystem;
using DataMedic.Application.ControlSystems.Queries.GetAllControlSystems;
using DataMedic.Application.ControlSystems.Queries.GetControlSystemById;
using DataMedic.Application.ControlSystems.Queries.GetControlSystemsWithPagination;
using DataMedic.Contracts.Common;
using DataMedic.Contracts.ControlSystems;
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
/// Control Systems Controller
/// </summary>
[ApiVersion("1.0")]
public class ControlSystemsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates Control Systems Controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="mapper"></param>
    public ControlSystemsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets paginated list of control systems
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.ControlSystems.Get)]
    //[Authorize(Policy = PolicyNames.RequireExpertPolicy)]
    [ProducesResponseType(typeof(PagedResponse<ControlSystemResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] GetControlSystemsQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetControlSystemsWithPaginationQuery>(queryParameters);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            controlSystems => Ok(_mapper.Map<PagedResponse<ControlSystemResponse>>(controlSystems)),
            Problem
        );
    }

    /// <summary>
    /// Gets all control systems
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.ControlSystems.GetAll)]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<ControlSystemResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var query = new GetAllControlSystemsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            value => Ok(_mapper.Map<IReadOnlyCollection<ControlSystemResponse>>(value)),
            Problem
        );
    }

    /// <summary>
    /// Gets a control system by ID
    /// </summary>
    /// <param name="controlSystemId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.ControlSystems.GetById)]
    [ProducesResponseType(typeof(ControlSystemResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid controlSystemId,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetControlSystemByIdQuery>(controlSystemId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            controlSystem => Ok(_mapper.Map<ControlSystemResponse>(controlSystem)),
            Problem
        );
    }

    /// <summary>
    /// Creates a control system
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.ControlSystems.Create)]
    [ProducesResponseType(typeof(ControlSystemResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateControlSystemRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<CreateControlSystemCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            controlSystem =>
                CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { ControlSystemId = controlSystem.Id.Value },
                    value: _mapper.Map<ControlSystemResponse>(controlSystem)
                ),
            Problem
        );
    }

    /// <summary>
    /// Updates a control system
    /// </summary>
    /// <param name="controlSystemId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.ControlSystems.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid controlSystemId,
        UpdateControlSystemRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<UpdateControlSystemCommand>((controlSystemId, request));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Deletes a control system
    /// </summary>
    /// <param name="controlSystemId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.ControlSystems.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid controlSystemId,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<DeleteControlSystemCommand>(controlSystemId);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}