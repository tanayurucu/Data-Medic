using DataMedic.Application.Departments.Commands.CreateDepartment;
using DataMedic.Application.Departments.Commands.DeleteDepartment;
using DataMedic.Application.Departments.Commands.UpdateDepartment;
using DataMedic.Application.Departments.Queries.GetDepartmentById;
using DataMedic.Application.Departments.Queries.GetDepartmentsWithPagination;
using DataMedic.Contracts.Departments;
using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;

using Mapster;

namespace DataMedic.Presentation.Common.Mappings;

/// <summary>
/// Mappings for Department
/// </summary>
public sealed class DepartmentMappings : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Department, DepartmentResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<GetDepartmentsQueryParameters, GetDepartmentsWithPaginationQuery>();

        config.NewConfig<CreateDepartmentRequest, CreateDepartmentCommand>();

        config.NewConfig<Guid, GetDepartmentByIdQuery>().Map(dest => dest.DepartmentId, src => src);

        config
            .NewConfig<Guid, DeleteDepartmentCommand>()
            .Map(dest => dest.DepartmentId, src => src);

        config
            .NewConfig<
                (Guid DepartmentId, UpdateDepartmentRequest request),
                UpdateDepartmentCommand
            >()
            .Map(dest => dest.Name, src => src.request.Name)
            .Map(dest => dest.DepartmentId, src => src.DepartmentId);
    }
}
