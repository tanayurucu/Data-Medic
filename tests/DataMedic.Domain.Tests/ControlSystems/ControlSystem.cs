using Bogus;

using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;

using FluentAssertions;

namespace DataMedic.Domain.Tests.ControlSystems;

public class ControlSystemTests
{
    [Fact]
    public void ControlSystem_Should_BeCreatedWithValidName()
    {
        // Arrange
        var controlSystemName =
            new Faker<ControlSystemName>()
                .UsePrivateConstructor()
                .RuleFor(m => m.Value, f => f.Name.FindName())
                .Generate();

        // Act
        var controlSystem = ControlSystem.Create(controlSystemName);

        // Assert
        controlSystem.Name.Should().NotBeNull();
        controlSystem.Name.Should().Be(controlSystemName);
    }

    [Fact]
    public void ControlSystem_Should_UpdateNameWhenSetNameIsCalled()
    {
        // Arrange
        var controlSystemNameCreator = new Faker<ControlSystemName>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, f => f.Name.FindName());
        var oldControlSystemName = controlSystemNameCreator.Generate();
        var newControlSystemName = controlSystemNameCreator.Generate();
        var controlSystem = ControlSystem.Create(oldControlSystemName);

        // Act
        controlSystem.SetName(newControlSystemName);

        // Assert
        controlSystem.Name.Should().NotBeNull();
        controlSystem.Name.Should().NotBe(oldControlSystemName);
        controlSystem.Name.Should().Be(newControlSystemName);
    }
}