using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.Departments.Commands.UpdateDepartment;

public sealed class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithError(Errors.Department.Id.Empty)
            .NotEqual(Guid.Empty)
            .WithError(Errors.Department.Id.Empty);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.Department.Name.Empty)
            .MaximumLength(DepartmentName.MaxLength)
            .WithError(Errors.Department.Name.TooLong);
    }
}