using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Departments.Commands.UpdateDepartment;

public sealed record UpdateDepartmentCommand(Guid DepartmentId, string Name) : ICommand<ErrorOr<Updated>>;