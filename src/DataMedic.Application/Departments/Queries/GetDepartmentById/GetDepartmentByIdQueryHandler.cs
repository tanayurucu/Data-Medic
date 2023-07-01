using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Departments.Queries.GetDepartmentById;

internal sealed class GetDepartmentByIdQueryHandler : IQueryHandler<GetDepartmentByIdQuery, ErrorOr<Department>>
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<ErrorOr<Department>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var departmentId = DepartmentId.Create(request.DepartmentId);

        if (await _departmentRepository.FindByIdAsync(departmentId, cancellationToken) is not Department department)
        {
            return Errors.Department.NotFound(departmentId);
        }

        return department;
    }
}