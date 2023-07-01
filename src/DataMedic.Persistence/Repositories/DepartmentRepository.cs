using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Persistence.Common.Abstractions;
using DataMedic.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;

namespace DataMedic.Persistence.Repositories;

internal sealed class DepartmentRepository : AsyncRepository<Department, DepartmentId>, IDepartmentRepository
{
    public DepartmentRepository(DataMedicDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Department?> FindByNameAsync(DepartmentName name, CancellationToken cancellationToken = default)
    {
        return (from department in _dbContext.Set<Department>()
                where department.Name == name
                select department).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Paged<Department>> FindManyWithPaginationAsync(string searchString, int pageSize, int pageNumber,
        CancellationToken cancellationToken = default)
    {
        return (from department in _dbContext.Set<Department>()
                where string.IsNullOrEmpty(searchString) || ((string)department.Name).Contains(searchString)
                orderby department.Name
                select department).ToPagedListAsync(pageNumber, pageSize, cancellationToken);
    }
}