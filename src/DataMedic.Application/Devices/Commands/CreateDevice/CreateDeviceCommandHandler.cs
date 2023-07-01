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

namespace DataMedic.Application.Devices.Commands.CreateDevice;

public sealed class CreateDeviceCommandHandler
    : ICommandHandler<CreateDeviceCommand, ErrorOr<Device>>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDeviceGroupRepository _deviceGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDeviceCommandHandler(
        IUnitOfWork unitOfWork,
        IDeviceRepository deviceRepository,
        IDepartmentRepository departmentRepository,
        IDeviceGroupRepository deviceGroupRepository
    )
    {
        _unitOfWork = unitOfWork;
        _deviceRepository = deviceRepository;
        _departmentRepository = departmentRepository;
        _deviceGroupRepository = deviceGroupRepository;
    }

    public async Task<ErrorOr<Device>> Handle(
        CreateDeviceCommand request,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<DeviceName> createDeviceNameResult = DeviceName.Create(request.Name);
        var description = request.Description;
        ErrorOr<InventoryNumber> createInventoryNumberResult = InventoryNumber.Create(
            request.InventoryNumber
        );
        var departmentId = DepartmentId.Create(request.DepartmentId);
        var deviceGroupId = DeviceGroupId.Create(request.DeviceGroupId);
        if (
            Errors.Combine(createDeviceNameResult, createInventoryNumberResult)
                is List<Error> errors
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

        if (
            await _deviceRepository.FindByNameAsync(createDeviceNameResult.Value, cancellationToken)
            is not null
        )
        {
            return Errors.Device.Name.AlreadyExists(request.Name);
        }

        if (
            await _deviceRepository.FindByInventoryNumber(
                createInventoryNumberResult.Value,
                cancellationToken
            )
            is not null
        )
        {
            return Errors.Device.InventoryNumber.AlreadyExists(request.InventoryNumber);
        }

        var device = Device.Create(
            createDeviceNameResult.Value,
            description,
            createInventoryNumberResult.Value,
            deviceGroupId,
            departmentId
        );
        await _deviceRepository.AddAsync(device, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return device;
    }
}
