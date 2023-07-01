using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Departments.Commands.DeleteDepartment;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;

using FluentAssertions;

using Moq;

namespace DataMedic.Application.Tests.Departments;

public class DeleteDepartmentCommandHandlerTests
{
    private readonly Mock<IDepartmentRepository> _departmentRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly DeleteDepartmentCommandHandler _handler;

    public DeleteDepartmentCommandHandlerTests()
    {
        _handler = new DeleteDepartmentCommandHandler(_departmentRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenDepartmentNotFound()
    {
        // Arrange
        var command = new DeleteDepartmentCommand(Guid.NewGuid());
        var departmentId = DepartmentId.Create(command.DepartmentId);

        _departmentRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<DepartmentId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Should().Be(Errors.Department.NotFound(departmentId));
    }

    [Fact]
    public async Task Handler_Should_RemoveDepartment()
    {
        // Arrange
        var department = Department.Create(DepartmentName.Create("Trial").Value);

        var command = new DeleteDepartmentCommand(department.Id.Value);

        _departmentRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<DepartmentId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(department);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();

        _departmentRepositoryMock.Verify(x => x.Remove(department), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}