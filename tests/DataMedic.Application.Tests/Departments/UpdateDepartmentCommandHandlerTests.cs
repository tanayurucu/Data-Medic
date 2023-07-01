using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Departments.Commands.UpdateDepartment;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;

using FluentAssertions;

using Moq;

namespace DataMedic.Application.Tests.Departments;

public class UpdateDepartmentCommandHandlerTests
{
    private readonly Mock<IDepartmentRepository> _departmentRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly UpdateDepartmentCommandHandler _handler;

    public UpdateDepartmentCommandHandlerTests()
    {
        _handler = new UpdateDepartmentCommandHandler(
            _departmentRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenDepartmentNotFound()
    {
        // Arrange
        var command = new UpdateDepartmentCommand(Guid.NewGuid(), "Trial");
        var departmentId = DepartmentId.Create(command.DepartmentId);
        _departmentRepositoryMock
            .Setup(x => x.FindByIdAsync(departmentId, default))
            .ReturnsAsync((Department?)null);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Department.NotFound(departmentId));
        _departmentRepositoryMock.Verify(x => x.FindByIdAsync(departmentId, default), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenNewNameIsInvalid()
    {
        // Arrange
        var department = Department.Create(DepartmentName.Create("Trial").Value);
        var command = new UpdateDepartmentCommand(department.Id.Value, string.Empty);
        _departmentRepositoryMock
            .Setup(x => x.FindByIdAsync(department.Id, default))
            .ReturnsAsync(department);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Department.Name.Empty);
        _departmentRepositoryMock.Verify(x => x.FindByIdAsync(department.Id, default), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenADepartmentAlreadyExistsWithNewName()
    {
        // Arrange
        var existingDepartmentWithNewName = Department.Create(DepartmentName.Create("Trial").Value);
        var department = Department.Create(DepartmentName.Create("Trial.1").Value);
        var command = new UpdateDepartmentCommand(department.Id.Value, "Trial");
        _departmentRepositoryMock
            .Setup(x => x.FindByIdAsync(department.Id, default))
            .ReturnsAsync(department);
        _departmentRepositoryMock
            .Setup(x => x.FindByNameAsync(existingDepartmentWithNewName.Name, default))
            .ReturnsAsync(existingDepartmentWithNewName);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError
            .Should()
            .Be(Errors.Department.AlreadyExists(existingDepartmentWithNewName.Name));
        _departmentRepositoryMock.Verify(x => x.FindByIdAsync(department.Id, default), Times.Once);
        _departmentRepositoryMock.Verify(
            x => x.FindByNameAsync(existingDepartmentWithNewName.Name, default),
            Times.Once
        );
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handler_Should_UpdateDepartment()
    {
        // Arrange
        var department = Department.Create(DepartmentName.Create("Trial").Value);
        var command = new UpdateDepartmentCommand(department.Id.Value, "Trial.2");
        var departmentName = DepartmentName.Create(command.Name).Value;
        _departmentRepositoryMock
            .Setup(x => x.FindByIdAsync(department.Id, default))
            .ReturnsAsync(department);
        _departmentRepositoryMock
            .Setup(x => x.FindByNameAsync(departmentName, default))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        _departmentRepositoryMock.Verify(x => x.FindByIdAsync(department.Id, default), Times.Once);
        _departmentRepositoryMock.Verify(
            x => x.FindByNameAsync(departmentName, default),
            Times.Once
        );
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        department.Name.Should().Be(departmentName);
    }
}