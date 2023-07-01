using DataMedic.Application.Common.Messages;
using DataMedic.Application.Common.Models;
using DataMedic.Domain.Departments;

using ErrorOr;

namespace DataMedic.Application.Departments.Queries.GetDepartmentsWithPagination;

public record GetDepartmentsWithPaginationQuery(string SearchString, int PageSize, int PageNumber)
    : IQuery<ErrorOr<Paged<Department>>>;