using DataMedic.Application.Sensors.Commands.CreateSensor;
using DataMedic.Application.Sensors.Commands.DeleteSensor;
using DataMedic.Application.Sensors.Commands.UpdateSensor;
using DataMedic.Application.Sensors.Models;
using DataMedic.Application.Sensors.Queries.GetAllSensors;
using DataMedic.Application.Sensors.Queries.GetSensorById;
using DataMedic.Application.Sensors.Queries.GetSensorTree;
using DataMedic.Contracts.Common;
using DataMedic.Contracts.Routes;
using DataMedic.Contracts.Sensors;
using DataMedic.Infrastructure.Authentication.Constants;
using DataMedic.Presentation.Controllers.Base;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataMedic.Presentation.Controllers.V1;

/// <summary>
/// Sensors Controller
/// </summary>
[ApiVersion("1.0")]
public class SensorsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates Sensors Controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="mapper"></param>
    public SensorsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a sensor
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.Sensors.Create)]
    [ProducesResponseType(typeof(SensorWithDetailResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateSensorRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<CreateSensorCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            sensor =>
                CreatedAtAction(
                    actionName: nameof(Create),
                    routeValues: new { SensorId = sensor.Id.Value },
                    value: _mapper.Map<SensorWithDetailResponse>(sensor)
                ),
            Problem
        );
    }

    /// <summary>
    /// Gets all sensors at deviceComponent
    /// </summary>
    /// <param name="deviceComponentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Sensors.Get)]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<SensorWithDetailResponse>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAllByComponentId(
        Guid deviceComponentId,
        CancellationToken cancellationToken = default
    )
    {
        var query = new GetAllSensorsQuery(deviceComponentId);
        var result = await _sender.Send(query, cancellationToken);
        return result.Match(
            sensors => Ok(_mapper.Map<IReadOnlyCollection<SensorWithSensorDetailResponse>>(sensors)),
            Problem
        );
    }

    /// <summary>
    /// Deletes a Sensor
    /// </summary>
    /// <param name="sensorId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.Sensors.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid sensorId,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<DeleteSensorCommand>(sensorId);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Updates a sensor
    /// </summary>
    /// <param name="sensorId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.Sensors.Update)]
    public async Task<IActionResult> Update(
        Guid sensorId,
        UpdateSensorRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = _mapper.Map<UpdateSensorCommand>((sensorId, request));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Gets a sensor by ID
    /// </summary>
    /// <param name="sensorId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Sensors.GetById)]
    public async Task<IActionResult> GetById(
        Guid sensorId,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetSensorByIdQuery>(sensorId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            value => Ok(_mapper.Map<SensorWithSensorDetailResponse>(value)),
            Problem
        );
    }

    /// <summary>
    /// Gets sensor tree
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Sensors.GetTree)]
    public async Task<IActionResult> GetSensorTree(
        [FromQuery] GetSensorTreeQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        var query = _mapper.Map<GetSensorTreeQuery>(queryParameters);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(value => Ok(_mapper.Map<SensorTreeResponse>(value)), Problem);
    }
}
