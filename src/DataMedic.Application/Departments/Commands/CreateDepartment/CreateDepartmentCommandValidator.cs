using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments.ValueObjects;

using FluentValidation;

namespace DataMedic.Application.Departments.Commands.CreateDepartment;

public sealed class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(Errors.Department.Name.Empty)
            .MaximumLength(DepartmentName.MaxLength)
            .WithError(Errors.Department.Name.TooLong);
    }
}