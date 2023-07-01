using DataMedic.Presentation.Controllers.Base;
using DataMedic.Contracts.Routes;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using DataMedic.Contracts.Components;
using DataMedic.Application.Components.Queries.GetComponentsWithPagination;
using DataMedic.Contracts.Common;
using DataMedic.Application.Components.Queries.GetAllComponents;
using DataMedic.Application.Components.Queries.GetComponentById;
using DataMedic.Application.Components.Commands.CreateComponent;
using DataMedic.Application.Components.Commands.UpdateComponent;
using DataMedic.Application.Components.Commands.DeleteComponent;

namespace DataMedic.Presentation.Controllers.V1;

/// <summary>
/// Components Controller
/// </summary>
[ApiVersion("1.0")]
public class ComponentsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates a Components Controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="mapper"></param>
    public ComponentsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets paginated list of components
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Components.Get)]
    public async Task<IActionResult> Get(
        [FromQuery] GetComponentsWithPaginationQueryParameters queryParameters,
        CancellationToken cancellationToken
    )
    {
        var query = _mapper.Map<GetComponentsWithPaginationQuery>(queryParameters);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            value => Ok(_mapper.Map<PagedResponse<ComponentResponse>>(value)),
            error => Problem(error)
        );
    }

    /// <summary>
    /// Gets all Components
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Components.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllComponentsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            value => Ok(_mapper.Map<IReadOnlyCollection<ComponentResponse>>(value)),
            error => Problem(error)
        );
    }

    /// <summary>
    /// Gets a component by ID
    /// </summary>
    /// <param name="componentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Components.GetById)]
    public async Task<IActionResult> GetById(Guid componentId, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetComponentByIdQuery>(componentId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            value => Ok(_mapper.Map<ComponentResponse>(value)),
            error => Problem(error)
        );
    }

    /// <summary>
    /// Creates a component
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.Components.Create)]
    public async Task<IActionResult> Create(
        CreateComponentRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = _mapper.Map<CreateComponentCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            value =>
                CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { ComponentId = value.Id.Value },
                    value: _mapper.Map<ComponentResponse>(value)
                ),
            error => Problem(error)
        );
    }

    /// <summary>
    /// Updates a Component
    /// </summary>
    /// <param name="componentId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.Components.Update)]
    public async Task<IActionResult> Update(
        Guid componentId,
        UpdateComponentRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = _mapper.Map<UpdateComponentCommand>((componentId, request));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Deletes a component
    /// </summary>
    /// <param name="componentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.Components.Delete)]
    public async Task<IActionResult> Delete(Guid componentId, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<DeleteComponentCommand>(componentId);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}
