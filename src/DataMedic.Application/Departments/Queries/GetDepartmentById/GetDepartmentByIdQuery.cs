using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Departments;

using ErrorOr;

namespace DataMedic.Application.Departments.Queries.GetDepartmentById;

public sealed record GetDepartmentByIdQuery(Guid DepartmentId) : IQuery<ErrorOr<Department>>;