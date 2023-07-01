using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Devices.Commands.UpdateDevice;

internal sealed class UpdateDeviceCommandHandler
    : ICommandHandler<UpdateDeviceCommand, ErrorOr<Updated>>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDeviceGroupRepository _deviceGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDeviceCommandHandler(
        IDeviceRepository deviceRepository,
        IUnitOfWork unitOfWork,
        IDepartmentRepository departmentRepository,
        IDeviceGroupRepository deviceGroupRepository
    )
    {
        _deviceRepository = deviceRepository;
        _unitOfWork = unitOfWork;
        _departmentRepository = departmentRepository;
        _deviceGroupRepository = deviceGroupRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateDeviceCommand request,
        CancellationToken cancellationToken
    )
    {
        var deviceId = DeviceId.Create(request.DeviceId);
        ErrorOr<DeviceName> createDeviceNameResult = DeviceName.Create(request.Name);
        var description = request.Description;
        ErrorOr<InventoryNumber> createInventoryNumberResult = InventoryNumber.Create(
            request.InventoryNumber
        );
        var deviceGroupId = DeviceGroupId.Create(request.DeviceGroupId);
        var departmentId = DepartmentId.Create(request.DepartmentId);
        if (
            Errors.Combine(createDeviceNameResult, createInventoryNumberResult) is var errors
            && errors.Any()
        )
        {
            return errors;
        }

        if (
            await _departmentRepository.FindByIdAsync(departmentId, cancellationToken)
            is not Department department
        )
        {
            return Errors.Department.NotFound(departmentId);
        }

        if (
            await _deviceGroupRepository.FindByIdAsync(deviceGroupId, cancellationToken)
            is not DeviceGroup deviceGroup
        )
        {
            return Errors.DeviceGroup.NotFound(deviceGroupId);
        }

        if (await _deviceRepository.FindByIdAsync(deviceId, cancellationToken) is not Device device)
        {
            return Errors.Device.NotFound;
        }

        if (
            await _deviceRepository.FindByInventoryNumber(
                createInventoryNumberResult.Value,
                cancellationToken
            )
                is Device existingDeviceWithInv
            && existingDeviceWithInv.Id != device.Id
        )
        {
            return Errors.Device.InventoryNumber.AlreadyExists(request.InventoryNumber);
        }

        if (
            await _deviceRepository.FindByNameAsync(createDeviceNameResult.Value, cancellationToken)
                is Device existingDeviceWithName
            && existingDeviceWithName.Id != device.Id
        )
        {
            return Errors.Device.Name.AlreadyExists(request.Name);
        }

        device.SetDescription(description);
        device.SetName(createDeviceNameResult.Value);
        device.SetInventoryNumber(createInventoryNumberResult.Value);
        device.SetDeviceGroup(deviceGroupId);
        device.SetDepartment(departmentId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
