using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;

using ErrorOr;

namespace DataMedic.Application.Departments.Commands.DeleteDepartment;

public sealed class DeleteDepartmentCommandHandler : ICommandHandler<DeleteDepartmentCommand, ErrorOr<Deleted>>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDepartmentCommandHandler(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
    {
        _departmentRepository = departmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var departmentId = DepartmentId.Create(request.DepartmentId);
        if (await _departmentRepository.FindByIdAsync(departmentId, cancellationToken) is not Department department)
        {
            return Errors.Department.NotFound(departmentId);
        }

        _departmentRepository.Remove(department);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}