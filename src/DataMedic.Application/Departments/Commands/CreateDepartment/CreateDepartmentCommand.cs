using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Departments;

using ErrorOr;

namespace DataMedic.Application.Departments.Commands.CreateDepartment;

public sealed record CreateDepartmentCommand(string Name) : ICommand<ErrorOr<Department>>;