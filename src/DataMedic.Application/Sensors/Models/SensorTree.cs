using DataMedic.Domain.Components;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.Departments;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Application.Sensors.Models;

using OperatingSystem = Domain.OperatingSystems.OperatingSystem;

public sealed record SensorTree(SensorTreeStatistics Statistics, List<SensorTreeDevice> Devices);

public sealed record SensorTreeStatistics(int UpCount, int DownCount, int InactiveCount);

public sealed record SensorTreeDevice(
    DeviceId Id,
    DeviceName Name,
    InventoryNumber InventoryNumber,
    DeviceGroup DeviceGroup,
    Department Department,
    SensorTreeDeviceComponent DeviceComponent
);

public sealed record SensorTreeDeviceComponent(
    DeviceComponentId Id,
    Component Component,
    IpAddress IpAddress,
    OperatingSystem OperatingSystem,
    ControlSystem ControlSystem,
    List<SensorTreeSensor> Sensors
);

public sealed record SensorTreeSensor(SensorId Id, SensorType Type, bool IsActive, bool Status);
