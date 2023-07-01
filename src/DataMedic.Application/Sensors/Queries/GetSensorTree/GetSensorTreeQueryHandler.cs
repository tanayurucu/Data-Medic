using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Sensors.Models;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Sensors.Queries.GetSensorTree;

public sealed class GetSensorTreeQueryHandler
    : IQueryHandler<GetSensorTreeQuery, ErrorOr<SensorTree>>
{
    private readonly IControlSystemRepository _controlSystemRepository;
    private readonly IOperatingSystemRepository _operatingSystemRepository;
    private readonly IDeviceGroupRepository _deviceGroupRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ISensorRepository _sensorRepository;

    public GetSensorTreeQueryHandler(
        IControlSystemRepository controlSystemRepository,
        IOperatingSystemRepository operatingSystemRepository,
        IDeviceGroupRepository deviceGroupRepository,
        IDepartmentRepository departmentRepository,
        ISensorRepository sensorRepository
    )
    {
        _controlSystemRepository = controlSystemRepository;
        _operatingSystemRepository = operatingSystemRepository;
        _deviceGroupRepository = deviceGroupRepository;
        _departmentRepository = departmentRepository;
        _sensorRepository = sensorRepository;
    }

    public async Task<ErrorOr<SensorTree>> Handle(
        GetSensorTreeQuery request,
        CancellationToken cancellationToken
    )
    {
        var controlSystemId = ControlSystemId.Create(request.ControlSystemId);
        var operatingSystemId = OperatingSystemId.Create(request.OperatingSystemId);
        var deviceGroupId = DeviceGroupId.Create(request.DeviceGroupId);
        var departmentId = DepartmentId.Create(request.DepartmentId);
        if (
            controlSystemId.Value != Guid.Empty
            && await _controlSystemRepository.FindByIdAsync(controlSystemId, cancellationToken)
                is null
        )
        {
            return Errors.ControlSystem.NotFound;
        }

        if (
            operatingSystemId.Value != Guid.Empty
            && await _operatingSystemRepository.FindByIdAsync(operatingSystemId, cancellationToken)
                is null
        )
        {
            return Errors.OperatingSystem.NotFound;
        }

        if (
            deviceGroupId.Value != Guid.Empty
            && await _deviceGroupRepository.FindByIdAsync(deviceGroupId, cancellationToken) is null
        )
        {
            return Errors.DeviceGroup.NotFound(deviceGroupId);
        }

        if (
            departmentId.Value != Guid.Empty
            && await _departmentRepository.FindByIdAsync(departmentId, cancellationToken) is null
        )
        {
            return Errors.Department.NotFound(departmentId);
        }

        return await _sensorRepository.GetSensorTreeAsync(
            request.ShowOnlyUpSensors,
            request.ShowOnlyDownSensors,
            request.ShowOnlyInactiveSensors,
            request.SearchString,
            controlSystemId,
            operatingSystemId,
            deviceGroupId,
            departmentId,
            cancellationToken
        );
    }
}
