using DataMedic.Application.Common.Models;
using DataMedic.Application.Emails.Models;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.Emails;
using DataMedic.Domain.Emails.ValueObjects;

using DataMedic.Application.Emails.Models;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Application.Common.Interfaces.Persistence.Repositories;

public interface IEmailRepository : IAsyncRepository<Email, EmailId>
{
    Task<Email?> FindByAddressAndDepartmentAsync(
        EmailAddress address,
        DepartmentId departmentId,
        CancellationToken cancellationToken = default
    );
    Task<Paged<EmailWithDepartment>> FindManyWithPaginationAsync(
        string searchString,
        int pageNumber,
        int pageSize,
        DepartmentId departmentId,
        CancellationToken cancellationToken = default
    );
    Task<List<EmailAddress>> GetMailingListForDepartmentAsync(
        DepartmentId departmentId,
        CancellationToken cancellationToken = default
    );

    Task<List<EmailAddress>> GetMailingListForSensorIdAsync(SensorId sensorSensorId, CancellationToken cancellationToken = default);
}