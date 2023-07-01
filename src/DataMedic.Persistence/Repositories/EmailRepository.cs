using Microsoft.EntityFrameworkCore;

using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Models;
using DataMedic.Application.Emails.Models;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Emails;
using DataMedic.Domain.Emails.ValueObjects;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence.Common.Abstractions;
using DataMedic.Persistence.Common.Extensions;

namespace DataMedic.Persistence.Repositories;

internal sealed class EmailRepository : AsyncRepository<Email, EmailId>, IEmailRepository
{
    public EmailRepository(DataMedicDbContext dbContext)
        : base(dbContext) { }

    public Task<Email?> FindByAddressAndDepartmentAsync(
        EmailAddress address,
        DepartmentId departmentId,
        CancellationToken cancellationToken = default
    ) =>
        (
            from email in _dbContext.Set<Email>()
            where email.Address == address
            where email.DepartmentId == departmentId
            select email
        ).FirstOrDefaultAsync(cancellationToken);

    public Task<Paged<EmailWithDepartment>> FindManyWithPaginationAsync(
        string searchString,
        int pageNumber,
        int pageSize,
        DepartmentId departmentId,
        CancellationToken cancellationToken = default
    ) =>
        (
            from email in _dbContext.Set<Email>()
            join department in _dbContext.Set<Department>()
                on email.DepartmentId equals department.Id
            where departmentId.Value == Guid.Empty || email.DepartmentId == departmentId
            where
                string.IsNullOrEmpty(searchString) || ((string)email.Address).Contains(searchString)
            orderby email.Address
            select new EmailWithDepartment(
                email.Id,
                email.Address,
                department,
                email.CreatedOnUtc,
                email.ModifiedOnUtc
            )
        ).ToPagedListAsync(pageNumber, pageSize, cancellationToken);

    public Task<List<EmailAddress>> GetMailingListForDepartmentAsync(
        DepartmentId departmentId,
        CancellationToken cancellationToken = default
    ) =>
        (
            from email in _dbContext.Set<Email>()
            where email.DepartmentId == departmentId
            select email.Address
        ).ToListAsync(cancellationToken);

    public async Task<List<EmailAddress>> GetMailingListForSensorIdAsync(SensorId sensorId, CancellationToken cancellationToken = default)
    {
        var sensor = await _dbContext.Set<Sensor>().FirstOrDefaultAsync(a => a.Id == sensorId, cancellationToken: cancellationToken);
        var device = await _dbContext.Set<Device>().FirstOrDefaultAsync(a => a.Components.Any(c=>c.Id == sensor.DeviceComponentId), cancellationToken: cancellationToken);
        return await _dbContext.Set<Email>().Where(a => a.DepartmentId == device.DepartmentId).Select(a => a.Address)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
