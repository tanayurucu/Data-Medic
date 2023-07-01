using DataMedic.Application.Common.Messages;
using DataMedic.Application.Devices.Models;
using DataMedic.Domain.Devices;

using ErrorOr;

namespace DataMedic.Application.Devices.Queries.GetAllDevices;

public sealed record GetAllDevicesQuery() : IQuery<ErrorOr<List<DeviceWithDetails>>>;
