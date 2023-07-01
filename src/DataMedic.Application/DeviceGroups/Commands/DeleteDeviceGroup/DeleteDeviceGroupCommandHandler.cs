using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Commands.DeleteDeviceGroup;

public sealed class DeleteDeviceGroupCommandHandler : ICommandHandler<DeleteDeviceGroupCommand, ErrorOr<Deleted>>
{
    private readonly IDeviceGroupRepository _deviceGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDeviceGroupCommandHandler(IDeviceGroupRepository deviceGroupRepository, IUnitOfWork unitOfWork)
    {
        _deviceGroupRepository = deviceGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteDeviceGroupCommand request, CancellationToken cancellationToken)
    {
        var deviceGroupId = DeviceGroupId.Create(request.DeviceGroupId);
        if (await _deviceGroupRepository.FindByIdAsync(deviceGroupId, cancellationToken) is not DeviceGroup deviceGroup)
        {
            return Errors.DeviceGroup.NotFound(deviceGroupId);
        }

        _deviceGroupRepository.Remove(deviceGroup);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}