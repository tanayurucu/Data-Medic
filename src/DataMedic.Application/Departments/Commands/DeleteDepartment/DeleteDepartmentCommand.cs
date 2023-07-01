using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Departments.Commands.DeleteDepartment;

public sealed record DeleteDepartmentCommand(Guid DepartmentId) : ICommand<ErrorOr<Deleted>>;