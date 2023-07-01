using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;

using FluentAssertions;

namespace DataMedic.Domain.Tests.Departments;

public class DepartmentTests
{
    [Fact]
    public void Department_Should_BeCreatedWithValidDepartmentName()
    {
        // Arrange
        var departmentNameResult = DepartmentName.Create("Trial");

        // Act
        var department = Department.Create(departmentNameResult.Value);

        // Assert
        department.Name.Should().Be(departmentNameResult.Value);
    }

    [Fact]
    public void Department_Should_UpdateDepartmentName()
    {
        // Arrange
        var departmentName = DepartmentName.Create("Trial.1");
        var newDepartmentName = DepartmentName.Create("Trial.2");
        var department = Department.Create(departmentName.Value);

        // Act
        department.SetName(newDepartmentName.Value);

        // Assert
        department.Name.Should().Be(newDepartmentName.Value);
    }
}