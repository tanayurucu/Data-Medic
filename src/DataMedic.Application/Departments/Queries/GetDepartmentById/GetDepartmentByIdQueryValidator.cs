using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Departments.Queries.GetDepartmentById;

public sealed class GetDepartmentByIdQueryValidator : AbstractValidator<GetDepartmentByIdQuery>
{
    public GetDepartmentByIdQueryValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithError(Errors.Department.Id.Empty)
            .NotEqual(Guid.Empty)
            .WithError(Errors.Department.Id.Empty);
    }
}