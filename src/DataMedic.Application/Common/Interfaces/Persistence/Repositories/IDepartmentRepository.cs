using DataMedic.Application.Common.Models;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;

namespace DataMedic.Application.Common.Interfaces.Persistence.Repositories;

public interface IDepartmentRepository : IAsyncRepository<Department, DepartmentId>
{
    Task<Department?> FindByNameAsync(
        DepartmentName name,
        CancellationToken cancellationToken = default
    );

    Task<Paged<Department>> FindManyWithPaginationAsync(
        string searchString,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default
    );
}