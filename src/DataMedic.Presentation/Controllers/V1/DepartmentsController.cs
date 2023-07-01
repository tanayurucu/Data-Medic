using DataMedic.Application.Departments.Commands.CreateDepartment;
using DataMedic.Application.Departments.Commands.DeleteDepartment;
using DataMedic.Application.Departments.Commands.UpdateDepartment;
using DataMedic.Application.Departments.Queries.GetAllDepartments;
using DataMedic.Application.Departments.Queries.GetDepartmentById;
using DataMedic.Application.Departments.Queries.GetDepartmentsWithPagination;
using DataMedic.Contracts.Common;
using DataMedic.Contracts.Departments;
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
public class DepartmentsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates Departments Controller
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="mapper"></param>
    public DepartmentsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets paginated list of departments
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Departments.GetWithPagination)]
    //[Authorize(Policy = PolicyNames.RequireExpertPolicy)]
    [ProducesResponseType(typeof(PagedResponse<DepartmentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] GetDepartmentsQueryParameters queryParameters,
        CancellationToken cancellationToken = default)
    {
        var query = _mapper.Map<GetDepartmentsWithPaginationQuery>(queryParameters);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(departments => Ok(_mapper.Map<PagedResponse<DepartmentResponse>>(departments)),
            Problem);
    }

    /// <summary>
    /// Gets all departments
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Departments.GetAll)]
    [ProducesResponseType(typeof(IReadOnlyCollection<DepartmentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var query = new GetAllDepartmentsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(departments => Ok(_mapper.Map<IReadOnlyCollection<DepartmentResponse>>(departments)), Problem);
    }

    /// <summary>
    /// Gets a department by ID
    /// </summary>
    /// <param name="departmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.Departments.GetById)]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid departmentId, CancellationToken cancellationToken = default)
    {
        var query = _mapper.Map<GetDepartmentByIdQuery>(departmentId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(department => Ok(_mapper.Map<DepartmentResponse>(department)), Problem);
    }

    /// <summary>
    /// Creates a department
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.Departments.Create)]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<CreateDepartmentCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(department =>
                CreatedAtAction(
                    actionName: nameof(GetById),
                    routeValues: new { DepartmentId = department.Id.Value },
                    value: _mapper.Map<DepartmentResponse>(department)),
            Problem);
    }

    /// <summary>
    /// Updates a department
    /// </summary>
    /// <param name="departmentId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut(ApiRoutes.Departments.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(Guid departmentId, UpdateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<UpdateDepartmentCommand>((departmentId, request));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }

    /// <summary>
    /// Deletes a department
    /// </summary>
    /// <param name="departmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete(ApiRoutes.Departments.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid departmentId, CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<DeleteDepartmentCommand>(departmentId);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), Problem);
    }
}