using Bogus;

using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices.ValueObjects;

using FluentAssertions;

namespace DataMedic.Domain.Tests.Devices.ValueObjects;

public class DeviceNameTests
{
    [Fact]
    public void DeviceName_Should_Not_BeCreatedWithEmptyString()
    {
        // Arrange
        var deviceName = string.Empty;

        // Act
        var createDeviceNameResult = DeviceName.Create(deviceName);

        // Assert
        createDeviceNameResult.IsError.Should().BeTrue();
        createDeviceNameResult.Errors.Should().ContainSingle();
        createDeviceNameResult.FirstError.Should().Be(Errors.Device.Name.Empty);
    }

    [Fact]
    public void DeviceName_Should_Not_BeCreatedWithWhitespace()
    {
        // Arrange
        var deviceName = " ";

        // Act
        var createDeviceNameResult = DeviceName.Create(deviceName);

        // Assert
        createDeviceNameResult.IsError.Should().BeTrue();
        createDeviceNameResult.Errors.Should().ContainSingle();
        createDeviceNameResult.FirstError.Should().Be(Errors.Device.Name.Empty);
    }

    [Fact]
    public void DeviceName_Should_Not_BeCreatedWithTooLongString()
    {
        // Arrange
        var deviceName = new Faker().Random.String2(DeviceName.MaxLength + 1);

        // Act
        var createDeviceNameResult = DeviceName.Create(deviceName);

        // Assert
        createDeviceNameResult.IsError.Should().BeTrue();
        createDeviceNameResult.Errors.Should().ContainSingle();
        createDeviceNameResult.FirstError.Should().Be(Errors.Device.Name.TooLong);
    }

    [Fact]
    public void DeviceName_Should_BeCreatedWithValidString()
    {
        // Arrange
        var deviceName = new Faker().Random.String2(1, DeviceName.MaxLength);

        // Act
        var createDeviceNameResult = DeviceName.Create(deviceName);

        // Assert
        createDeviceNameResult.IsError.Should().BeFalse();
        createDeviceNameResult.Value.Value.Should().Be(deviceName);
    }
}