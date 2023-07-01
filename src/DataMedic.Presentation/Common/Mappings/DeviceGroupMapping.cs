using DataMedic.Application.DeviceGroups.Commands.CreateDeviceGroup;
using DataMedic.Application.DeviceGroups.Commands.DeleteDeviceGroup;
using DataMedic.Application.DeviceGroups.Commands.UpdateDeviceGroup;
using DataMedic.Application.DeviceGroups.Queries.GetDeviceGroupById;
using DataMedic.Application.DeviceGroups.Queries.GetDeviceGroupsWithPagination;
using DataMedic.Contracts.DeviceGroups;
using DataMedic.Domain.DeviceGroups;

using Mapster;

namespace DataMedic.Presentation.Common.Mappings;

/// <summary>
/// Mappings for Device Group
/// </summary>
public sealed class DeviceGroupMapping : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<DeviceGroup, DeviceGroupResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<GetDeviceGroupQueryParameters, GetDeviceGroupsWithPaginationQuery>();

        config.NewConfig<CreateDeviceGroupRequest, CreateDeviceGroupCommand>();

        config
            .NewConfig<Guid, GetDeviceGroupByIdQuery>()
            .Map(dest => dest.DeviceGroupId, src => src);

        config
            .NewConfig<Guid, DeleteDeviceGroupCommand>()
            .Map(dest => dest.DeviceGroupId, src => src);

        config
            .NewConfig<
                (Guid DeviceGroupId, UpdateDeviceGroupRequest request),
                UpdateDeviceGroupCommand
            >()
            .Map(dest => dest.Name, src => src.request.Name)
            .Map(dest => dest.DeviceGroupId, src => src.DeviceGroupId);
    }
}
