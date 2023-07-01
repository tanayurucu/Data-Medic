using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithError(Errors.Department.Id.Empty)
            .NotEqual(Guid.Empty)
            .WithError(Errors.Department.Id.Empty);
    }
}