using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.DeviceGroups.Queries.GetDeviceGroupById
{
    internal sealed class GetDeviceGroupByIdQueryHandler : IQueryHandler<GetDeviceGroupByIdQuery, ErrorOr<DeviceGroup>>
    {
        private readonly IDeviceGroupRepository _deviceGroupRepository;

        public GetDeviceGroupByIdQueryHandler(IDeviceGroupRepository deviceGroupRepository)
        {
            _deviceGroupRepository = deviceGroupRepository;
        }

        public async Task<ErrorOr<DeviceGroup>> Handle(GetDeviceGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var deviceGroupId = DeviceGroupId.Create(request.DeviceGroupId);

            if (await _deviceGroupRepository.FindByIdAsync(deviceGroupId, cancellationToken) is not DeviceGroup deviceGroup)
            {
                return Errors.DeviceGroup.NotFound(deviceGroupId);
            }
            return deviceGroup;
        }
    }
}