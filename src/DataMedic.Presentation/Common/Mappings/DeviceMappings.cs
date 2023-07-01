using DataMedic.Application.Devices.Commands.AddDeviceComponent;
using DataMedic.Application.Devices.Commands.CreateDevice;
using DataMedic.Application.Devices.Commands.DeleteDevice;
using DataMedic.Application.Devices.Commands.DeleteDeviceComponent;
using DataMedic.Application.Devices.Commands.UpdateDevice;
using DataMedic.Application.Devices.Commands.UpdateDeviceComponent;
using DataMedic.Application.Devices.Models;
using DataMedic.Application.Devices.Queries.GetAllDeviceComponents;
using DataMedic.Application.Devices.Queries.GetDeviceById;
using DataMedic.Application.Devices.Queries.GetDeviceComponentsWithPagination;
using DataMedic.Application.Devices.Queries.GetDevicesWithPagination;
using DataMedic.Contracts.Devices;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.Entities;

using Mapster;

namespace DataMedic.Presentation.Common.Mappings;

/// <summary>
/// Mappings for Device
/// </summary>
public sealed class DeviceMappings : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Device, DeviceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.InventoryNumber, src => src.InventoryNumber.Value)
            .Map(dest => dest.DeviceGroupId, src => src.DeviceGroupId.Value)
            .Map(dest => dest.DepartmentId, src => src.DepartmentId.Value);

        config.NewConfig<GetDevicesQueryParameters, GetDevicesWithPaginationQuery>();

        config
            .NewConfig<
                (Guid DeviceId, GetDeviceComponentsQueryParameters QueryParameters),
                GetDeviceComponentsWithPaginationQuery
            >()
            .Map(dest => dest.DeviceId, src => src.DeviceId)
            .Map(dest => dest.SearchString, src => src.QueryParameters.SearchString)
            .Map(dest => dest.PageNumber, src => src.QueryParameters.PageNumber)
            .Map(dest => dest.OperatingSystemId, src => src.QueryParameters.OperatingSystemId)
            .Map(dest => dest.ControlSystemId, src => src.QueryParameters.ControlSystemId)
            .Map(dest => dest.PageSize, src => src.QueryParameters.PageSize);

        config.NewConfig<CreateDeviceRequest, CreateDeviceCommand>();

        config.NewConfig<Guid, GetDeviceByIdQuery>().Map(dest => dest.DeviceId, src => src);

        config.NewConfig<Guid, DeleteDeviceCommand>().Map(dest => dest.DeviceId, src => src);

        config
            .NewConfig<(Guid DeviceId, UpdateDeviceRequest request), UpdateDeviceCommand>()
            .Map(dest => dest.Name, src => src.request.Name)
            .Map(dest => dest.DeviceId, src => src.DeviceId)
            .Map(dest => dest.Description, src => src.request.Description)
            .Map(dest => dest.InventoryNumber, src => src.request.InventoryNumber)
            .Map(dest => dest.DeviceGroupId, src => src.request.DeviceGroupId)
            .Map(dest => dest.DepartmentId, src => src.request.DepartmentId);

        config
            .NewConfig<DeviceComponent, DeviceComponentResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ComponentId, src => src.ComponentId.Value)
            .Map(dest => dest.OperatingSystemId, src => src.OperatingSystemId.Value)
            .Map(dest => dest.IpAddress, src => src.IpAddress.Value)
            .Map(dest => dest.ControlSystemId, src => src.ControlSystemId.Value);
        config
            .NewConfig<
                (Guid DeviceId, AddDeviceComponentRequest request),
                AddDeviceComponentCommand
            >()
            .Map(dest => dest.DeviceId, src => src.DeviceId)
            .Map(dest => dest.ComponentId, src => src.request.ComponentId)
            .Map(dest => dest.OperatingSystemId, src => src.request.OperatingSystemId)
            .Map(dest => dest.IpAddress, src => src.request.IpAddress)
            .Map(dest => dest.ControlSystemId, src => src.request.ControlSystemId);
        config
            .NewConfig<
                (Guid DeviceId, Guid DeviceComponentId, UpdateDeviceComponentRequest request),
                UpdateDeviceComponentCommand
            >()
            .Map(dest => dest.DeviceId, src => src.DeviceId)
            .Map(dest => dest.DeviceComponentId, src => src.DeviceComponentId)
            .Map(dest => dest.OperatingSystemId, src => src.request.OperatingSystemId)
            .Map(dest => dest.IpAddress, src => src.request.IpAddress)
            .Map(dest => dest.ControlSystemId, src => src.request.ControlSystemId);

        config
            .NewConfig<DeviceWithDetails, DeviceWithDetailsResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.InventoryNumber, src => src.InventoryNumber.Value)
            .Map(dest => dest.DeviceGroup, src => src.DeviceGroup)
            .Map(dest => dest.Department, src => src.Department);

        config
            .NewConfig<DeviceComponentWithDetails, DeviceComponentWithDetailsResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Component, src => src.Component)
            .Map(dest => dest.IpAddress, src => src.IpAddress.Value)
            .Map(dest => dest.OperatingSystem, src => src.OperatingSystem)
            .Map(dest => dest.ControlSystem, src => src.ControlSystem);

        config
            .NewConfig<(Guid deviceId, Guid deviceComponentId), DeleteDeviceComponentCommand>()
            .Map(dest => dest.DeviceId, src => src.deviceId)
            .Map(dest => dest.DeviceComponentId, src => src.deviceComponentId);

        config
            .NewConfig<Guid, GetAllDeviceComponentsQuery>()
            .Map(dest => dest.DeviceId, src => src);
    }
}
