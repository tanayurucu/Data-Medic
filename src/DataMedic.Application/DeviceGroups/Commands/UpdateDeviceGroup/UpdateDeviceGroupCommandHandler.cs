using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Commands.UpdateDeviceGroup;

public sealed class UpdateDeviceGroupCommandHandler : ICommandHandler<UpdateDeviceGroupCommand, ErrorOr<Updated>>
{
    private readonly IDeviceGroupRepository _deviceGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDeviceGroupCommandHandler(IDeviceGroupRepository deviceGroupRepository, IUnitOfWork unitOfWork)
    {
        _deviceGroupRepository = deviceGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateDeviceGroupCommand request, CancellationToken cancellationToken)
    {
        var deviceGroupId = DeviceGroupId.Create(request.DeviceGroupId);
        if (await _deviceGroupRepository.FindByIdAsync(deviceGroupId, cancellationToken) is not DeviceGroup deviceGroup)
        {
            return Errors.DeviceGroup.NotFound(deviceGroupId);
        }

        ErrorOr<DeviceGroupName> createDeviceGroupNameResult = DeviceGroupName.Create(request.Name);
        if (createDeviceGroupNameResult.IsError)
        {
            return createDeviceGroupNameResult.Errors;
        }

        if (await _deviceGroupRepository.FindByNameAsync(createDeviceGroupNameResult.Value, cancellationToken) is DeviceGroup existingDeviceGroup && existingDeviceGroup.Id != deviceGroupId)
        {
            return Errors.DeviceGroup.Name.AlreadyExists(createDeviceGroupNameResult.Value);
        }

        deviceGroup.SetName(createDeviceGroupNameResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}