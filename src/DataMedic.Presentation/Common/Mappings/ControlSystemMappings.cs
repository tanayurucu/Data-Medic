using DataMedic.Application.ControlSystems.Commands.CreateControlSystem;
using DataMedic.Application.ControlSystems.Commands.DeleteControlSystem;
using DataMedic.Application.ControlSystems.Commands.UpdateControlSystem;
using DataMedic.Application.ControlSystems.Queries.GetControlSystemById;
using DataMedic.Application.ControlSystems.Queries.GetControlSystemsWithPagination;
using DataMedic.Contracts.ControlSystems;
using DataMedic.Domain.ControlSystems;

using Mapster;

namespace DataMedic.Presentation.Common.Mappings;

/// <summary>
/// Mappings for ControlSystem
/// </summary>
public sealed class ControlSystemMappings : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<ControlSystem, ControlSystemResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<GetControlSystemsQueryParameters, GetControlSystemsWithPaginationQuery>();

        config.NewConfig<CreateControlSystemRequest, CreateControlSystemCommand>();

        config
            .NewConfig<Guid, GetControlSystemByIdQuery>()
            .Map(dest => dest.ControlSystemId, src => src);

        config
            .NewConfig<Guid, DeleteControlSystemCommand>()
            .Map(dest => dest.ControlSystemId, src => src);

        config
            .NewConfig<
                (Guid ControlSystemId, UpdateControlSystemRequest request),
                UpdateControlSystemCommand
            >()
            .Map(dest => dest.Name, src => src.request.Name)
            .Map(dest => dest.ControlSystemId, src => src.ControlSystemId);
    }
}
