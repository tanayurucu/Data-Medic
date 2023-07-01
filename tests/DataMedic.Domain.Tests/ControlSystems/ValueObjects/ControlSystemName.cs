using Bogus;

using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.ControlSystems.ValueObjects;

using FluentAssertions;

namespace DataMedic.Domain.Tests.ControlSystems.ValueObjects;

public class ControlSystemNameTests
{
    [Fact]
    public void ControlSystemName_Should_Not_BeCreatedWithEmptyString()
    {
        // Arrange
        var controlSystemName = string.Empty;

        // Act
        var createControlSystemNameResult = ControlSystemName.Create(controlSystemName);

        // Assert
        createControlSystemNameResult.IsError.Should().BeTrue();
        createControlSystemNameResult.Errors.Should().ContainSingle();
        createControlSystemNameResult.FirstError.Should().Be(Errors.ControlSystem.Name.Empty);
    }

    [Fact]
    public void ControlSystemName_Should_NotBeCreatedWithWhitespace()
    {
        // Arrange
        var controlSystemName = " ";

        // Act
        var createControlSystemNameResult = ControlSystemName.Create(controlSystemName);

        // Assert
        createControlSystemNameResult.IsError.Should().BeTrue();
        createControlSystemNameResult.Errors.Should().ContainSingle();
        createControlSystemNameResult.FirstError.Should().Be(Errors.ControlSystem.Name.Empty);
    }

    [Fact]
    public void ControlSystemName_Should_NotBeCreatedWithTooLongString()
    {
        // Arrange
        var controlSystemName = new Faker().Random.String2(ControlSystemName.MaxLength + 1);

        // Act
        var createControlSystemNameResult = ControlSystemName.Create(controlSystemName);

        // Assert
        createControlSystemNameResult.IsError.Should().BeTrue();
        createControlSystemNameResult.Errors.Should().ContainSingle();
        createControlSystemNameResult.FirstError.Should().Be(Errors.ControlSystem.Name.TooLong);
    }

    [Fact]
    public void ControlSystemName_Should_BeCreatedWithValidString()
    {
        // Arrange
        var controlSystemName = new Faker().Random.String2(1, ControlSystemName.MaxLength);

        // Act
        var createControlSystemNameResult = ControlSystemName.Create(controlSystemName);

        // Assert
        createControlSystemNameResult.IsError.Should().BeFalse();
        createControlSystemNameResult.Value.Value.Should().Be(controlSystemName);
    }
}