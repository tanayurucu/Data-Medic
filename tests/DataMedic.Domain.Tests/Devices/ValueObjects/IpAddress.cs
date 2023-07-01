using Bogus;

using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices.ValueObjects;

using FluentAssertions;

namespace DataMedic.Domain.Tests.Devices.ValueObjects;

public class IpAddressTests
{
    [Fact]
    public void IpAddress_Should_Not_BeCreatedWithEmptyString()
    {
        // Arrange
        var ipAddress = string.Empty;

        // Act
        var createIpAddressResult = IpAddress.Create(ipAddress);

        // Assert
        createIpAddressResult.IsError.Should().BeTrue();
        createIpAddressResult.Errors.Should().ContainSingle();
        createIpAddressResult.FirstError.Should().Be(Errors.Device.IpAddress.Empty);
    }

    [Fact]
    public void IpAddress_Should_Not_BeCreatedWithWhitespace()
    {
        // Arrange
        var ipAddress = " ";

        // Act
        var createIpAddressResult = IpAddress.Create(ipAddress);

        // Assert
        createIpAddressResult.IsError.Should().BeTrue();
        createIpAddressResult.Errors.Should().ContainSingle();
        createIpAddressResult.FirstError.Should().Be(Errors.Device.IpAddress.Empty);
    }

    [Fact]
    public void IpAddress_Should_Not_BeCreatedWithTooLongString()
    {
        // Arrange
        var ipAddress = new Faker().Random.String2(IpAddress.MaxLength + 1);

        // Act
        var createIpAddressResult = IpAddress.Create(ipAddress);

        // Assert
        createIpAddressResult.IsError.Should().BeTrue();
        createIpAddressResult.Errors.Should().ContainSingle();
        createIpAddressResult.FirstError.Should().Be(Errors.Device.IpAddress.Invalid);
    }

    [Fact]
    public void IpAddress_Should_Not_BeCreatedWithInvalidString()
    {
        // Arrange
        var ipAddress = new Faker().Random.String2(1, IpAddress.MaxLength - 1);

        // Act
        var createIpAddressResult = IpAddress.Create(ipAddress);

        // Assert
        createIpAddressResult.IsError.Should().BeTrue();
        createIpAddressResult.Errors.Should().ContainSingle();
        createIpAddressResult.FirstError.Should().Be(Errors.Device.IpAddress.Invalid);
    }

    [Fact]
    public void IpAddress_Should_BeCreatedWithValidIpAddress()
    {
        // Arrange
        var ipAddress = new Faker().Internet.IpAddress().ToString();

        // Act
        var createIpAddressResult = IpAddress.Create(ipAddress);

        // Assert
        createIpAddressResult.IsError.Should().BeFalse();
        createIpAddressResult.Value.Value.Should().Be(ipAddress);
    }
}