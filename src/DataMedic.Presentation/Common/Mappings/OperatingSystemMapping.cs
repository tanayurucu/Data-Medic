using DataMedic.Application.OperatingSystems.Commands.CreateOperatingSystem;
using DataMedic.Application.OperatingSystems.Commands.DeleteOperatingSystem;
using DataMedic.Application.OperatingSystems.Commands.UpdateOperatingSystem;
using DataMedic.Application.OperatingSystems.Queries.GetOperatingSystemById;
using DataMedic.Application.OperatingSystems.Queries.GetOperatingSystemsWithPagination;
using DataMedic.Contracts.OperatingSystems;
using DataMedic.Domain.OperatingSystems;
using DataMedic.Domain.OperatingSystems.ValueObjects;

using Mapster;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Presentation.Common.Mappings;

/// <summary>
/// Mappings for OperatingSystem
/// </summary>
public sealed class OperatingSystemMappings : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<OperatingSystem, OperatingSystemResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<GetOperatingSystemsQueryParameters, GetOperatingSystemsWithPaginationQuery>();

        config.NewConfig<CreateOperatingSystemRequest, CreateOperatingSystemCommand>();

        config.NewConfig<Guid, GetOperatingSystemByIdQuery>().Map(dest => dest.OperatingSystemId, src => src);

        config.NewConfig<Guid, DeleteOperatingSystemCommand>().Map(dest => dest.OperatingSystemId, src => src);

        config
            .NewConfig<(Guid OperatingSystemId, UpdateOperatingSystemRequest request), UpdateOperatingSystemCommand>()
            .Map(dest => dest.Name, src => src.request.Name)
            .Map(dest => dest.OperatingSystemId, src => src.OperatingSystemId);
    }
}