using DataMedic.Application.Devices.Commands.AddDeviceComponent;
using DataMedic.Application.Devices.Commands.CreateDevice;
using DataMedic.Application.Devices.Commands.DeleteDevice;
using DataMedic.Application.Devices.Commands.DeleteDeviceComponent;
using DataMedic.Application.Devices.Commands.UpdateDevice;
using DataMedic.Application.Devices.Commands.UpdateDeviceComponent;
using DataMedic.Application.Devices.Queries.GetAllDeviceComponents;
using DataMedic.Application.Devices.Queries.GetAllDevices;
using DataMedic.Application.Devices.Queries.GetDeviceById;
using DataMedic.Application.Devices.Queries.GetDeviceComponentsWithPagination;
using DataMedic.Application.Devices.Queries.GetDevicesWithPagination;
using DataMedic.Contracts.Common;
using DataMedic.Contracts.Devices;
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
/// Devices Controller
/// </summary>
[ApiVersion("1.0")]
public class DevicesController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates Devices Controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="mapper"></param>
    public DevicesController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets paginated list of devices
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Devices.Get)]
    //[Authorize(Policy = PolicyNames.RequireExpertPolicy)]
    [ProducesResponseType(
        typeof(PagedResponse<DeviceWithDetailsResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> Get(
        [FromQuery] GetDevicesQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetDevicesWithPaginationQuery>(queryParameters);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            devices => Ok(_mapper.Map<PagedResponse<DeviceWithDetailsResponse>>(devices)),
            Problem
        );
    }

    /// <summary>
    /// Gets all devices
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Devices.GetAll)]
    //[Authorize(Policy = PolicyNames.RequireExpertPolicy)]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<DeviceWithDetailsResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var query = new GetAllDevicesQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            devices => Ok(_mapper.Map<IReadOnlyCollection<DeviceWithDetailsResponse>>(devices)),
            Problem
        );
    }

    /// <summary>
    /// Gets paginated list of device components of a device
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Devices.GetComponents)]
    //[Authorize(Policy = PolicyNames.RequireExpertPolicy)]
    [ProducesResponseType(
        typeof(PagedResponse<DeviceComponentWithDetailsResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetComponents(
        Guid deviceId,
        [FromQuery] GetDeviceComponentsQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetDeviceComponentsWithPaginationQuery>(
            (deviceId, queryParameters)
        );

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            deviceComponent =>
                Ok(_mapper.Map<PagedResponse<DeviceComponentWithDetailsResponse>>(deviceComponent)),
            Problem
        );
    }

    /// <summary>
    /// Gets all device components for a device
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Devices.GetAllComponents)]
    //[Authorize(Policy = PolicyNames.RequireExpertPolicy)]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<DeviceComponentWithDetailsResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAllComponents(
        Guid deviceId,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetAllDeviceComponentsQuery>(deviceId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            deviceComponent =>
                Ok(
                    _mapper.Map<IReadOnlyCollection<DeviceComponentWithDetailsResponse>>(
                        deviceComponent
                    )
                ),
            Problem
        );
    }

    /// <summary>
    /// Gets a device by ID
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Devices.GetById)]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid deviceId,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetDeviceByIdQuery>(deviceId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(device => Ok(_mapper.Map<DeviceResponse>(device)), Problem);
    }

    /// <summary>
    /// Creates a device
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.Devices.Create)]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateDeviceRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<CreateDeviceCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            device =>
                CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { DeviceId = device.Id.Value },
                    value: _mapper.Map<DeviceResponse>(device)
                ),
            Problem
        );
    }

    /// <summary>
    /// Creates a device component for a device
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.Devices.AddDeviceComponent)]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddDeviceComponent(
        Guid deviceId,
        AddDeviceComponentRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<AddDeviceComponentCommand>((deviceId, request));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            deviceComponent =>
                CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { DeviceId = deviceId },
                    value: _mapper.Map<DeviceComponentResponse>(deviceComponent)
                ),
            Problem
        );
    }

    /// <summary>
    /// Updates a device
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.Devices.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid deviceId,
        UpdateDeviceRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<UpdateDeviceCommand>((deviceId, request));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Updates a device component
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="deviceComponentId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.Devices.UpdateDeviceComponent)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateDeviceComponent(
        Guid deviceId,
        Guid deviceComponentId,
        UpdateDeviceComponentRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<UpdateDeviceComponentCommand>(
            (deviceId, deviceComponentId, request)
        );

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Deletes a device
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.Devices.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid deviceId,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<DeleteDeviceCommand>(deviceId);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Deletes device component
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="deviceComponentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.Devices.DeleteDeviceComponent)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDeviceComponent(
        Guid deviceId,
        Guid deviceComponentId,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<DeleteDeviceComponentCommand>((deviceId, deviceComponentId));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match((_) => NoContent(), Problem);
    }
}
