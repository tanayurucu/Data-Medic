using DataMedic.Application.DeviceGroups.Commands.CreateDeviceGroup;
using DataMedic.Application.DeviceGroups.Commands.DeleteDeviceGroup;
using DataMedic.Application.DeviceGroups.Commands.UpdateDeviceGroup;
using DataMedic.Application.DeviceGroups.Queries.GetAllDeviceGroups;
using DataMedic.Application.DeviceGroups.Queries.GetDeviceGroupById;
using DataMedic.Application.DeviceGroups.Queries.GetDeviceGroupsWithPagination;
using DataMedic.Contracts.Common;
using DataMedic.Contracts.DeviceGroups;
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
/// Departments Controller
/// </summary>
[ApiVersion("1.0")]
public class DeviceGroupsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates DeviceGroups Controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="mapper"></param>
    public DeviceGroupsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets paginated list of device groups
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.DeviceGroups.Get)]
    //[Authorize(Policy = PolicyNames.RequireExpertPolicy)]
    [ProducesResponseType(typeof(PagedResponse<DeviceGroupResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] GetDeviceGroupQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetDeviceGroupsWithPaginationQuery>(queryParameters);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            deviceGroups => Ok(_mapper.Map<PagedResponse<DeviceGroupResponse>>(deviceGroups)),
            Problem
        );
    }

    /// <summary>
    /// Gets all device groups
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.DeviceGroups.GetAll)]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<DeviceGroupResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var query = new GetAllDeviceGroupsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            deviceGroups => Ok(_mapper.Map<IReadOnlyCollection<DeviceGroupResponse>>(deviceGroups)),
            Problem
        );
    }

    /// <summary>
    /// Gets a device group by ID
    /// </summary>
    /// <param name="deviceGroupId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.DeviceGroups.GetById)]
    [ProducesResponseType(typeof(DeviceGroupResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid deviceGroupId,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetDeviceGroupByIdQuery>(deviceGroupId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            deviceGroup => Ok(_mapper.Map<DeviceGroupResponse>(deviceGroup)),
            Problem
        );
    }

    /// <summary>
    /// Creates a device group
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.DeviceGroups.Create)]
    [ProducesResponseType(typeof(DeviceGroupResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateDeviceGroupRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<CreateDeviceGroupCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            deviceGroup =>
                CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { DeviceGroupId = deviceGroup.Id.Value },
                    value: _mapper.Map<DeviceGroupResponse>(deviceGroup)
                ),
            Problem
        );
    }

    /// <summary>
    /// Updates a device group
    /// </summary>
    /// <param name="deviceGroupId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.DeviceGroups.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid deviceGroupId,
        UpdateDeviceGroupRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<UpdateDeviceGroupCommand>((deviceGroupId, request));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Deletes a device group
    /// </summary>
    /// <param name="deviceGroupId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.DeviceGroups.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid deviceGroupId,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<DeleteDeviceGroupCommand>(deviceGroupId);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}