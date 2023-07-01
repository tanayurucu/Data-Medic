using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Commands.CreateDeviceGroup
{
    public sealed class CreateDeviceGroupCommandHandler : ICommandHandler<CreateDeviceGroupCommand, ErrorOr<DeviceGroup>>
    {
        private readonly IDeviceGroupRepository _deviceGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDeviceGroupCommandHandler(IDeviceGroupRepository deviceGroupRepository, IUnitOfWork unitOfWork)
        {
            _deviceGroupRepository = deviceGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<DeviceGroup>> Handle(CreateDeviceGroupCommand request, CancellationToken cancellationToken)
        {
            ErrorOr<DeviceGroupName> createDeviceGroupNameResult = DeviceGroupName.Create(request.Name);
            if (createDeviceGroupNameResult.IsError)
            {
                return createDeviceGroupNameResult.Errors;
            }

            if (await _deviceGroupRepository.FindByNameAsync(createDeviceGroupNameResult.Value, cancellationToken) is not null)
            {
                return Errors.DeviceGroup.Name.AlreadyExists(createDeviceGroupNameResult.Value);
            }

            var deviceGroup = DeviceGroup.Create(createDeviceGroupNameResult.Value);
            await _deviceGroupRepository.AddAsync(deviceGroup, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return deviceGroup;
        }
    }
}