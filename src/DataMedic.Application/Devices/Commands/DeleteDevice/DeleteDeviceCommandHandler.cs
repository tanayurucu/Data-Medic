using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Devices.Commands.DeleteDevice;

internal sealed class DeleteDeviceCommandHandler : ICommandHandler<DeleteDeviceCommand, ErrorOr<Deleted>>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDeviceCommandHandler(IDeviceRepository deviceRepository, IUnitOfWork unitOfWork)
    {
        _deviceRepository = deviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var deviceId = DeviceId.Create(request.DeviceId);
        if (await _deviceRepository.FindByIdAsync(deviceId, cancellationToken) is not Device device)
        {
            return Errors.Device.NotFound;
        }
        _deviceRepository.Remove(device);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}