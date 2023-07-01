// using DataMedic.Application.Common.Interfaces.Persistence;
// using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
// using DataMedic.Application.Common.Messages;
// using DataMedic.Application.Sensors.Commands.CreateSensor;
// using DataMedic.Application.Sensors.Models;
// using DataMedic.Domain.Common.Errors;
// using DataMedic.Domain.ControlSystems.ValueObjects;
// using DataMedic.Domain.Departments.ValueObjects;
// using DataMedic.Domain.DeviceGroups.ValueObjects;
// using DataMedic.Domain.Devices;
// using DataMedic.Domain.Devices.Entities;
// using DataMedic.Domain.Devices.ValueObjects;
// using DataMedic.Domain.Hosts;
// using DataMedic.Domain.Hosts.ValueObjects;
// using DataMedic.Domain.OperatingSystems.ValueObjects;
// using DataMedic.Domain.Sensors;
// using DataMedic.Domain.Sensors.Entities;
// using DataMedic.Domain.Sensors.ValueObjects;

// using ErrorOr;

// using Moq;

// using Xunit;

// namespace DataMedic.Application.Tests.Sensors.Commands.CreateSensor
// {
//     public class CreateSensorCommandHandlerTests
//     {
//         private readonly Mock<ISensorRepository> _sensorRepositoryMock;
//         private readonly Mock<IDeviceRepository> _deviceRepositoryMock;
//         private readonly Mock<IHostRepository> _hostRepositoryMock;
//         private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//         private readonly CreateSensorCommandHandler _handler;

//         public CreateSensorCommandHandlerTests()
//         {
//             _sensorRepositoryMock = new Mock<ISensorRepository>();
//             _deviceRepositoryMock = new Mock<IDeviceRepository>();
//             _hostRepositoryMock = new Mock<IHostRepository>();
//             _unitOfWorkMock = new Mock<IUnitOfWork>();
//             _handler = new CreateSensorCommandHandler(_sensorRepositoryMock.Object, _unitOfWorkMock.Object,
//                 _deviceRepositoryMock.Object, _hostRepositoryMock.Object);
//         }

//         [Fact]
//         public async Task Handle_WhenRequestIsValid_ShouldCreateSensorAndReturnSensorWithDetail()
//         {
//             // Arrange
//             var device = Device.Create(
//                 DeviceName.Create("test-name").Value
//                 , "test-desc"
//                 , InventoryNumber.Create("test-inv").Value
//                 , DeviceGroupId.CreateUnique()
//                 , DepartmentId.CreateUnique());
//             var cancellationToken = CancellationToken.None;
//             var deviceComponent = DeviceComponent.Create(DeviceComponentName.Create("test-component").Value, IpAddress.Create("0.0.0.0").Value, OperatingSystemId.CreateUnique(), ControlSystemId.CreateUnique());
//             var host = Host.Create(HostInformation.Create("test-hostName", 1111).Value, null, null);
//             device.AddDeviceComponent(deviceComponent);
//             _deviceRepositoryMock.Setup(r => r.FindByIdAsync(It.IsAny<DeviceId>(), cancellationToken))
//                 .ReturnsAsync(device);
//             _hostRepositoryMock.Setup(r => r.FindByIdAsync(It.IsAny<HostId>(), cancellationToken)).ReturnsAsync(host);
//             _sensorRepositoryMock.Setup(r => r.AddMqttSensorAsync(It.IsAny<MqttSensor>(), cancellationToken))
//                 .Returns(Task.CompletedTask);
//             _sensorRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Sensor>(), cancellationToken))
//                 .Returns(Task.CompletedTask);
//             _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).Returns(Task.FromResult(1));
//             var request = new CreateSensorCommand
//             (
//                 SensorType.MQTT.Value, host.Id.Value, deviceComponent.Id.Value,
//                 "test-description",
//                 device.Id.Value,
//                 new CreateMqttSensorDetailCommand(TopicName: "test-topic-name", TimeSpan.FromSeconds(3))
//             );
//             // Act
//             var result = await _handler.Handle(request, cancellationToken);

//             // Assert
//             Assert.NotNull(result.Value);
//             Assert.Equal(SensorType.MQTT.Value, result.Value.Type);
//             Assert.Equal("test-description", result.Value.Description);
//         }
//     }
// }