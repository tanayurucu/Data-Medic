using Bogus;

using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using FluentAssertions;

namespace DataMedic.Domain.Tests.OperatingSystems.ValueObjects;

public class OperatingSystemNameTests
{
    [Fact]
    public void OperatingSystemName_Should_Not_BeCreatedWithEmptyString()
    {
        // Arrange
        var operatingSystemName = string.Empty;

        // Act
        var createOperatingSystemNameResult = OperatingSystemName.Create(operatingSystemName);

        // Assert
        createOperatingSystemNameResult.IsError.Should().BeTrue();
        createOperatingSystemNameResult.Errors.Should().ContainSingle();
        createOperatingSystemNameResult.FirstError.Should().Be(Errors.OperatingSystem.Name.Empty);
    }

    [Fact]
    public void OperatingSystemName_Should_Not_BeCreatedWithWhitespace()
    {
        // Arrange
        var operatingSystemName = " ";

        // Act
        var createOperatingSystemNameResult = OperatingSystemName.Create(operatingSystemName);

        // Assert
        createOperatingSystemNameResult.IsError.Should().BeTrue();
        createOperatingSystemNameResult.Errors.Should().ContainSingle();
        createOperatingSystemNameResult.FirstError.Should().Be(Errors.OperatingSystem.Name.Empty);
    }

    [Fact]
    public void OperatingSystemName_Should_Not_BeCreatedWithTooLongString()
    {
        // Arrange
        var operatingSystemName = new Faker().Random.String2(OperatingSystemName.MaxLength + 1);

        // Act
        var createOperatingSystemNameResult = OperatingSystemName.Create(operatingSystemName);

        // Assert
        createOperatingSystemNameResult.IsError.Should().BeTrue();
        createOperatingSystemNameResult.Errors.Should().ContainSingle();
        createOperatingSystemNameResult.FirstError.Should().Be(Errors.OperatingSystem.Name.TooLong);
    }

    [Fact]
    public void OperatingSystemName_Should_BeCreatedWithValidString()
    {
        // Arrange
        var operatingSystemName = new Faker().Random.String2(1, OperatingSystemName.MaxLength);

        // Act
        var createOperatingSystemNameResult = OperatingSystemName.Create(operatingSystemName);

        // Assert
        createOperatingSystemNameResult.IsError.Should().BeFalse();
        createOperatingSystemNameResult.Value.Value.Should().Be(operatingSystemName);
    }
}