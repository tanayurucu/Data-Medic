using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.Devices.Commands.UpdateDeviceComponent;

internal sealed class UpdateDeviceComponentCommandHandler
    : ICommandHandler<UpdateDeviceComponentCommand, ErrorOr<Updated>>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IOperatingSystemRepository _operatingSystemRepository;
    private readonly IControlSystemRepository _controlSystemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDeviceComponentCommandHandler(
        IDeviceRepository deviceRepository,
        IUnitOfWork unitOfWork,
        IOperatingSystemRepository operatingSystemRepository,
        IControlSystemRepository controlSystemRepository
    )
    {
        _deviceRepository = deviceRepository;
        _unitOfWork = unitOfWork;
        _operatingSystemRepository = operatingSystemRepository;
        _controlSystemRepository = controlSystemRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateDeviceComponentCommand request,
        CancellationToken cancellationToken
    )
    {
        var deviceId = DeviceId.Create(request.DeviceId);
        var deviceComponentId = DeviceComponentId.Create(request.DeviceComponentId);
        var operatingSystemId = OperatingSystemId.Create(request.OperatingSystemId);
        var controlSystemId = ControlSystemId.Create(request.ControlSystemId);
        ErrorOr<IpAddress> createIpAddressResult = IpAddress.Create(request.IpAddress);
        if (Errors.Combine(createIpAddressResult) is var errors && errors.Any())
        {
            return errors;
        }

        if (
            await _controlSystemRepository.FindByIdAsync(controlSystemId, cancellationToken)
            is not ControlSystem controlSystem
        )
        {
            return Errors.ControlSystem.NotFound;
        }

        if (
            await _operatingSystemRepository.FindByIdAsync(operatingSystemId, cancellationToken)
            is not OperatingSystem operatingSystem
        )
        {
            return Errors.OperatingSystem.NotFound;
        }

        if (await _deviceRepository.FindByIdAsync(deviceId, cancellationToken) is not Device device)
        {
            return Errors.Device.NotFound;
        }

        if (
            device.Components.FirstOrDefault(component => component.Id == deviceComponentId)
            is not DeviceComponent deviceComponent
        )
        {
            return Errors.Device.DeviceComponent.NotFound;
        }

        deviceComponent.SetIpAddress(createIpAddressResult.Value);
        deviceComponent.SetControlSystem(controlSystem);
        deviceComponent.SetOperatingSystem(operatingSystem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
