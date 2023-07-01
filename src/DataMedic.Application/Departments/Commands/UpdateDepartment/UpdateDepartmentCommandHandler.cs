using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Departments.Commands.UpdateDepartment;

public sealed class UpdateDepartmentCommandHandler : ICommandHandler<UpdateDepartmentCommand, ErrorOr<Updated>>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
    {
        _departmentRepository = departmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var departmentId = DepartmentId.Create(request.DepartmentId);
        if (await _departmentRepository.FindByIdAsync(departmentId, cancellationToken) is not Department department)
        {
            return Errors.Department.NotFound(departmentId);
        }

        ErrorOr<DepartmentName> createDepartmentNameResult = DepartmentName.Create(request.Name);
        if (createDepartmentNameResult.IsError)
        {
            return createDepartmentNameResult.Errors;
        }

        if (await _departmentRepository.FindByNameAsync(createDepartmentNameResult.Value, cancellationToken) is
                Department existingDepartment && existingDepartment.Id != departmentId)
        {
            return Errors.Department.AlreadyExists(createDepartmentNameResult.Value);
        }

        department.SetName(createDepartmentNameResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}