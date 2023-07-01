using DataMedic.Contracts.Components;
using DataMedic.Contracts.ControlSystems;
using DataMedic.Contracts.Departments;
using DataMedic.Contracts.DeviceGroups;
using DataMedic.Contracts.OperatingSystems;

namespace DataMedic.Contracts.Sensors;

public sealed record SensorTreeResponse(
    SensorTreeStatisticsResponse Statistics,
    List<SensorTreeDeviceResponse> Devices
);

public sealed record SensorTreeStatisticsResponse(int UpCount, int DownCount, int InactiveCount);

public sealed record SensorTreeDeviceResponse(
    Guid Id,
    string Name,
    string InventoryNumber,
    DeviceGroupResponse DeviceGroup,
    DepartmentResponse Department,
    SensorTreeDeviceComponentResponse DeviceComponent
);

public sealed record SensorTreeDeviceComponentResponse(
    Guid Id,
    ComponentResponse Component,
    string IpAddress,
    OperatingSystemResponse OperatingSystem,
    ControlSystemResponse ControlSystem,
    List<SensorTreeSensorResponse> Sensors
);

public sealed record SensorTreeSensorResponse(Guid Id, int Type, bool IsActive, bool Status);
