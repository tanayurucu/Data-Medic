using Bogus;

using DataMedic.Domain.OperatingSystems.ValueObjects;

using FluentAssertions;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Domain.Tests.OperatingSystems;

public class OperatingSystemTests
{
    [Fact]
    public void OperatingSystem_Should_BeCreatedWithValidName()
    {
        // Arrange
        var operatingSystemName = new Faker<OperatingSystemName>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, f => f.Name.FindName())
            .Generate();

        // Act
        var operatingSystem = OperatingSystem.Create(operatingSystemName);

        // Assert
        operatingSystem.Name.Should().NotBeNull();
        operatingSystem.Name.Should().Be(operatingSystemName);
    }

    [Fact]
    public void OperatingSystem_Should_UpdateNameWhenSetNameIsCalled()
    {
        // Arrange
        var operatingSystemNameGenerator = new Faker<OperatingSystemName>()
            .UsePrivateConstructor()
            .RuleFor(m => m.Value, f => f.Name.FindName());
        var oldOperatingSystemName = operatingSystemNameGenerator.Generate();
        var newOperatingSystemName = operatingSystemNameGenerator.Generate();
        var operatingSystem = OperatingSystem.Create(oldOperatingSystemName);

        // Act
        operatingSystem.SetName(newOperatingSystemName);

        // Assert
        operatingSystem.Name.Should().NotBeNull();
        operatingSystem.Name.Should().NotBe(oldOperatingSystemName);
        operatingSystem.Name.Should().Be(newOperatingSystemName);

    }
}