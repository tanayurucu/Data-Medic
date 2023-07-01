using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Application.Devices.Commands.DeleteDevice;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Devices.Commands.DeleteDeviceComponent;

internal sealed class DeleteDeviceComponentCommandHandler
    : ICommandHandler<DeleteDeviceComponentCommand, ErrorOr<Deleted>>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDeviceComponentCommandHandler(
        IDeviceRepository deviceRepository,
        IUnitOfWork unitOfWork
    )
    {
        _deviceRepository = deviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteDeviceComponentCommand request,
        CancellationToken cancellationToken
    )
    {
        var deviceId = DeviceId.Create(request.DeviceId);
        var deviceComponentId = DeviceComponentId.Create(request.DeviceComponentId);

        if (await _deviceRepository.FindByIdAsync(deviceId, cancellationToken) is not Device device)
        {
            return Errors.Device.NotFound;
        }

        var result = device.RemoveDeviceComponentById(deviceComponentId);
        if (result.IsError)
        {
            return result.Errors;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
