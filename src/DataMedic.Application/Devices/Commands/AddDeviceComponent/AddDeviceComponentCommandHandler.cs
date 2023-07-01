using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Components;
using DataMedic.Domain.Components.ValueObjects;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.Devices.Commands.AddDeviceComponent;

public sealed class AddDeviceComponentCommandHandler
    : ICommandHandler<AddDeviceComponentCommand, ErrorOr<DeviceComponent>>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IOperatingSystemRepository _operatingSystemRepository;
    private readonly IControlSystemRepository _controlSystemRepository;
    private readonly IComponentRepository _componentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddDeviceComponentCommandHandler(
        IUnitOfWork unitOfWork,
        IDeviceRepository deviceRepository,
        IOperatingSystemRepository operatingSystemRepository,
        IControlSystemRepository controlSystemRepository,
        IComponentRepository componentRepository
    )
    {
        _deviceRepository = deviceRepository;
        _operatingSystemRepository = operatingSystemRepository;
        _controlSystemRepository = controlSystemRepository;
        _unitOfWork = unitOfWork;
        _componentRepository = componentRepository;
    }

    public async Task<ErrorOr<DeviceComponent>> Handle(
        AddDeviceComponentCommand request,
        CancellationToken cancellationToken
    )
    {
        var deviceId = DeviceId.Create(request.DeviceId);
        var componentId = ComponentId.Create(request.ComponentId);
        var operatingSystemId = OperatingSystemId.Create(request.OperatingSystemId);
        var controlSystemId = ControlSystemId.Create(request.ControlSystemId);
        var createIpAddressResult = IpAddress.Create(request.IpAddress);
        if (Errors.Combine(createIpAddressResult) is var errors && errors.Any())
        {
            return errors;
        }

        if (
            await _componentRepository.FindByIdAsync(componentId, cancellationToken)
            is not Component component
        )
        {
            return Errors.Component.NotFound;
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

        var deviceComponent = DeviceComponent.Create(
            component,
            createIpAddressResult.Value,
            operatingSystem,
            controlSystem
        );

        var addDeviceComponentResult = device.AddDeviceComponent(deviceComponent);
        if (addDeviceComponentResult.IsError)
        {
            return addDeviceComponentResult.Errors;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return deviceComponent;
    }
}
