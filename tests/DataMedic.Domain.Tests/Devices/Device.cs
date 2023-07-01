using Bogus;

using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using FluentAssertions;

namespace DataMedic.Domain.Tests.Devices;

public class DeviceTests
{
    [Fact]
    public void Device_Should_Be_CreatedWithValidInformation()
    {
        // Arrange
        var deviceName = new Faker<DeviceName>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, f => f.Name.FindName())
            .Generate();
        var description = new Faker().Lorem.Lines();
        var inventoryNumber = new Faker<InventoryNumber>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, f => f.Random.String())
            .Generate();
        var deviceGroupId = DeviceGroupId.CreateUnique();
        var departmentId = DepartmentId.CreateUnique();

        // Act
        var device = Device.Create(
            deviceName,
            description,
            inventoryNumber,
            deviceGroupId,
            departmentId
        );

        // Assert
        device.Should().NotBeNull();
        device.Name.Should().Be(deviceName);
        device.Description.Should().Be(description);
        device.InventoryNumber.Should().Be(inventoryNumber);
        device.DeviceGroupId.Should().Be(deviceGroupId);
        device.DepartmentId.Should().Be(departmentId);
        device.Components.Should().BeEmpty();
    }

    [Fact]
    public void Device_Should_UpdateNameWhenSetNameIsCalled()
    {
        // Arrange
        var deviceNameFaker = new Faker<DeviceName>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, f => f.Lorem.Word());
        var device = new Faker<Device>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Name, _ => deviceNameFaker.Generate())
            .Generate();
        var oldName = device.Name;
        var newDeviceName = deviceNameFaker.Generate();

        // Act
        device.SetName(newDeviceName);

        // Assert
        device.Name.Should().NotBeNull();
        device.Name.Should().NotBe(oldName);
        device.Name.Should().Be(newDeviceName);
    }

    [Fact]
    public void Device_Should_UpdateDescriptionWhenSetDescriptionIsCalled()
    {
        // Arrange
        var device = new Faker<Device>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Description, f => f.Lorem.Lines())
            .Generate();
        var oldDescription = device.Description;
        var newDescription = new Faker().Lorem.Lines();

        // Act
        device.SetDescription(newDescription);

        // Assert
        device.Description.Should().NotBeNull();
        device.Description.Should().NotBe(oldDescription);
        device.Description.Should().Be(newDescription);
    }

    [Fact]
    public void Device_Should_UpdateInventoryNumberWhenSetInventoryNumberIsCalled()
    {
        // Arrange
        var inventoryNumberFaker = new Faker<InventoryNumber>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, f => f.Lorem.Word());
        var device = new Faker<Device>()
            .UsePrivateConstructor()
            .RuleFor(m => m.InventoryNumber, _ => inventoryNumberFaker.Generate())
            .Generate();
        var oldInventoryNumber = device.InventoryNumber;
        var newInventoryNumber = inventoryNumberFaker.Generate();

        // Act
        device.SetInventoryNumber(newInventoryNumber);

        // Assert
        device.InventoryNumber.Should().NotBeNull();
        device.InventoryNumber.Should().NotBe(oldInventoryNumber);
        device.InventoryNumber.Should().Be(newInventoryNumber);
    }

    [Fact]
    public void Device_Should_UpdateDeviceGroupWhenUpdateDeviceGroupIsCalled()
    {
        // Arrange
        var deviceGroupIdFaker = new Faker<DeviceGroupId>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, _ => Guid.NewGuid());
        var device = new Faker<Device>()
            .UsePrivateConstructor()
            .RuleFor(m => m.DeviceGroupId, _ => deviceGroupIdFaker.Generate())
            .Generate();
        var oldDeviceGroupId = device.DeviceGroupId;
        var newDeviceGroupId = deviceGroupIdFaker.Generate();

        // Act
        device.SetDeviceGroup(newDeviceGroupId);

        // Assert
        device.DeviceGroupId.Should().NotBeNull();
        device.DeviceGroupId.Should().NotBe(oldDeviceGroupId);
        device.DeviceGroupId.Should().Be(newDeviceGroupId);
    }

    [Fact]
    public void Device_Should_UpdateDepartmentWhenUpdateDepartmentIsCalled()
    {
        // Arrange
        var departmentIdFaker = new Faker<DepartmentId>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, _ => Guid.NewGuid());
        var device = new Faker<Device>()
            .UsePrivateConstructor()
            .RuleFor(m => m.DepartmentId, _ => departmentIdFaker.Generate())
            .Generate();
        var oldDepartmentId = device.DepartmentId;
        var newDepartmentId = departmentIdFaker.Generate();

        // Act
        device.SetDepartment(newDepartmentId);

        // Assert
        device.DepartmentId.Should().NotBeNull();
        device.DepartmentId.Should().NotBe(oldDepartmentId);
        device.DepartmentId.Should().Be(newDepartmentId);
    }

    [Fact]
    public void Device_Should_AddComponentWhenAddDeviceComponentIsCalled()
    {
        // Arrange
        var deviceComponent = new Faker<DeviceComponent>()
            .UsePrivateConstructor()
            .RuleFor(
                m => m.Id,
                _ =>
                    new Faker<DeviceComponentId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            )
            .RuleFor(
                m => m.IpAddress,
                _ =>
                    new Faker<IpAddress>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, f => f.Internet.Ip())
                        .Generate()
            )
            .RuleFor(
                m => m.OperatingSystemId,
                _ =>
                    new Faker<OperatingSystemId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            )
            .RuleFor(
                m => m.ControlSystemId,
                _ =>
                    new Faker<ControlSystemId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            )
            .Generate();

        var device = new Faker<Device>().UsePrivateConstructor().Generate();

        // Act
        var addDeviceComponentResult = device.AddDeviceComponent(deviceComponent);

        // Assert
        addDeviceComponentResult.IsError.Should().BeFalse();
        addDeviceComponentResult.Value.Should().Be(deviceComponent);
        device.Components.Should().ContainSingle();
        device.Components.Should().Contain(deviceComponent);
    }

    [Fact]
    public void Device_Should_ReturnErrorWhenAddDeviceComponentIsCalledWithAlreadyExistingComponentWithIpAddress()
    {
        // Arrange
        var ipAddress = new Faker<IpAddress>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, f => f.Internet.Ip())
            .Generate();
        var deviceComponentGenerator = new Faker<DeviceComponent>()
            .UsePrivateConstructor()
            .RuleFor(
                m => m.Id,
                _ =>
                    new Faker<DeviceComponentId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            )
            .RuleFor(m => m.IpAddress, _ => ipAddress)
            .RuleFor(
                m => m.OperatingSystemId,
                _ =>
                    new Faker<OperatingSystemId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            )
            .RuleFor(
                m => m.ControlSystemId,
                _ =>
                    new Faker<ControlSystemId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            );
        var deviceComponentToBeAdded = deviceComponentGenerator.Generate();
        var alreadyExistingDeviceComponent = deviceComponentGenerator.Generate();

        var device = new Faker<Device>().UsePrivateConstructor().Generate();
        device.AddDeviceComponent(alreadyExistingDeviceComponent);

        // Act
        var addDeviceComponentResult = device.AddDeviceComponent(deviceComponentToBeAdded);

        // Assert
        addDeviceComponentResult.IsError.Should().BeTrue();
        addDeviceComponentResult.Errors.Should().ContainSingle();
        addDeviceComponentResult.FirstError
            .Should()
            .Be(Errors.Device.IpAddress.AlreadyExists(ipAddress));
        device.Components.Should().ContainSingle();
        device.Components.Should().Contain(alreadyExistingDeviceComponent);
    }

    [Fact]
    public void Device_Should_RemoveComponentWhenRemoveDeviceComponentIsCalled()
    {
        // Arrange
        var deviceComponent = new Faker<DeviceComponent>()
            .UsePrivateConstructor()
            .RuleFor(
                m => m.Id,
                _ =>
                    new Faker<DeviceComponentId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            )
            .RuleFor(
                m => m.IpAddress,
                _ =>
                    new Faker<IpAddress>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, f => f.Internet.Ip())
                        .Generate()
            )
            .RuleFor(
                m => m.OperatingSystemId,
                _ =>
                    new Faker<OperatingSystemId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            )
            .RuleFor(
                m => m.ControlSystemId,
                _ =>
                    new Faker<ControlSystemId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            )
            .Generate();

        var device = new Faker<Device>().UsePrivateConstructor().Generate();
        device.AddDeviceComponent(deviceComponent);

        // Act
        var removeDeviceComponentResult = device.RemoveDeviceComponentById(deviceComponent.Id);

        // Assert
        removeDeviceComponentResult.IsError.Should().BeFalse();
        device.Components.Should().BeEmpty();
    }

    [Fact]
    public void Device_Should_ReturnErrorWhenRemoveDeviceComponentIsCalledWithNonExistingDeviceComponentId()
    {
        // Arrange
        var deviceComponentIdGenerator = new Faker<DeviceComponentId>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, _ => Guid.NewGuid());
        var deviceComponent = new Faker<DeviceComponent>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Id, _ => deviceComponentIdGenerator.Generate())
            .RuleFor(
                m => m.IpAddress,
                _ =>
                    new Faker<IpAddress>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, f => f.Internet.Ip())
                        .Generate()
            )
            .RuleFor(
                m => m.OperatingSystemId,
                _ =>
                    new Faker<OperatingSystemId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            )
            .RuleFor(
                m => m.ControlSystemId,
                _ =>
                    new Faker<ControlSystemId>()
                        .UsePrivateConstructor()
                        .RuleFor(m => m.Value, _ => Guid.NewGuid())
                        .Generate()
            )
            .Generate();

        var device = new Faker<Device>().UsePrivateConstructor().Generate();
        device.AddDeviceComponent(deviceComponent);
        var deviceComponentIdThatDoesNotExists = deviceComponentIdGenerator.Generate();

        // Act
        var removeDeviceComponentResult = device.RemoveDeviceComponentById(
            deviceComponentIdThatDoesNotExists
        );

        // Assert
        removeDeviceComponentResult.IsError.Should().BeTrue();
        removeDeviceComponentResult.Errors.Should().ContainSingle();
        removeDeviceComponentResult.FirstError.Should().Be(Errors.Device.DeviceComponent.NotFound);
        device.Components.Should().ContainSingle();
        device.Components.Should().Contain(deviceComponent);
    }
}
