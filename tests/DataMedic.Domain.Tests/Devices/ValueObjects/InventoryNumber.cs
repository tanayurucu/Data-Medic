using Bogus;

using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Devices.ValueObjects;

using FluentAssertions;

namespace DataMedic.Domain.Tests.Devices.ValueObjects;

public class InventoryNumberTests
{
    [Fact]
    public void InventoryNumber_Should_Not_BeCreatedWithEmptyString()
    {
        // Arrange
        var inventoryNumber = "";

        // Act
        var createInventoryNumberResult = InventoryNumber.Create(inventoryNumber);

        // Assert
        createInventoryNumberResult.IsError.Should().BeTrue();
        createInventoryNumberResult.Errors.Should().ContainSingle();
        createInventoryNumberResult.FirstError.Should().Be(Errors.Device.InventoryNumber.Empty);
    }

    [Fact]
    public void InventoryNumber_Should_Not_BeCreatedWithWhitespace()
    {
        // Arrange
        var inventoryNumber = " ";

        // Act
        var createInventoryNumberResult = InventoryNumber.Create(inventoryNumber);

        // Assert
        createInventoryNumberResult.IsError.Should().BeTrue();
        createInventoryNumberResult.Errors.Should().ContainSingle();
        createInventoryNumberResult.FirstError.Should().Be(Errors.Device.InventoryNumber.Empty);
    }

    [Fact]
    public void InventoryNumber_Should_Not_BeCreatedWithTooLongString()
    {
        // Arrange
        var inventoryNumber = new Faker().Lorem.Letter(InventoryNumber.MaxLength + 1);

        // Act
        var createInventoryNumberResult = InventoryNumber.Create(inventoryNumber);

        // Assert
        createInventoryNumberResult.IsError.Should().BeTrue();
        createInventoryNumberResult.Errors.Should().ContainSingle();
        createInventoryNumberResult.FirstError.Should().Be(Errors.Device.InventoryNumber.TooLong);
    }

    [Fact]
    public void InventoryNumber_Should_BeCreatedWithValidString()
    {
        // Arrange
        var inventoryNumber = new Faker().Random.String2(1, InventoryNumber.MaxLength);

        // Act
        var createInventoryNumberResult = InventoryNumber.Create(inventoryNumber);

        // Assert
        createInventoryNumberResult.IsError.Should().BeFalse();
        createInventoryNumberResult.Value.Value.Should().Be(inventoryNumber);
    }
}