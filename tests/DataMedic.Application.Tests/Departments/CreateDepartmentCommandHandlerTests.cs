using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Departments.Commands.CreateDepartment;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;

using FluentAssertions;

using Moq;

namespace DataMedic.Application.Tests.Departments;

public class CreateDepartmentCommandHandlerTests
{
    private readonly Mock<IDepartmentRepository> _departmentRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly CreateDepartmentCommandHandler _handler;

    public CreateDepartmentCommandHandlerTests()
    {
        _handler = new CreateDepartmentCommandHandler(
            _departmentRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenDepartmentNameCreationFails()
    {
        // Arrange
        var command = new CreateDepartmentCommand(string.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Should().Be(Errors.Department.Name.Empty);
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenDepartmentAlreadyExists()
    {
        // Arrange
        var command = new CreateDepartmentCommand("Trial");
        var departmentName = DepartmentName.Create(command.Name).Value;
        _departmentRepositoryMock
            .Setup(
                x => x.FindByNameAsync(It.IsAny<DepartmentName>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(Department.Create(departmentName));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Department.AlreadyExists(departmentName));
    }

    [Fact]
    public async Task Handler_Should_CreateDepartmentAndSaveToRepository()
    {
        // Arrange
        var command = new CreateDepartmentCommand("Trial");
        var departmentName = DepartmentName.Create("Trial").Value;

        _departmentRepositoryMock
            .Setup(
                x => x.FindByNameAsync(It.IsAny<DepartmentName>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(departmentName);

        _departmentRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}