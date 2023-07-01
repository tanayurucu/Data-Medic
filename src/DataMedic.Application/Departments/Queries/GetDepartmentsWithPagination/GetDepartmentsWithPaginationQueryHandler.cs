using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.Departments;

using ErrorOr;

namespace DataMedic.Application.Departments.Queries.GetDepartmentsWithPagination;

internal sealed class GetDepartmentsWithPaginationQueryHandler
    : IQueryHandler<GetDepartmentsWithPaginationQuery, ErrorOr<Paged<Department>>>
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentsWithPaginationQueryHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<ErrorOr<Paged<Department>>> Handle(
        GetDepartmentsWithPaginationQuery request,
        CancellationToken cancellationToken
    ) =>
        await _departmentRepository.FindManyWithPaginationAsync(
            request.SearchString,
            request.PageSize,
            request.PageNumber,
            cancellationToken
        );
}