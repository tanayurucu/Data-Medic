using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Departments;

using ErrorOr;

namespace DataMedic.Application.Departments.Queries.GetAllDepartments;

public sealed record GetAllDepartmentsQuery() : IQuery<ErrorOr<List<Department>>>;