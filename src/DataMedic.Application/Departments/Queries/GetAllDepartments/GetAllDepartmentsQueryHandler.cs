using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Departments;

using ErrorOr;

namespace DataMedic.Application.Departments.Queries.GetAllDepartments;

public sealed class GetAllDepartmentsQueryHandler : IQueryHandler<GetAllDepartmentsQuery, ErrorOr<List<Department>>>
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<ErrorOr<List<Department>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        return await _departmentRepository.FindAllAsync(cancellationToken);
    }
}