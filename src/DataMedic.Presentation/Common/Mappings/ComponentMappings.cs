using DataMedic.Application.Components.Commands.CreateComponent;
using DataMedic.Application.Components.Commands.DeleteComponent;
using DataMedic.Application.Components.Commands.UpdateComponent;
using DataMedic.Application.Components.Queries.GetComponentById;
using DataMedic.Contracts.Components;
using DataMedic.Domain.Components;

using Mapster;

namespace DataMedic.Presentation.Common.Mappings;

/// <summary>
/// Mappings for Component Aggregate
/// </summary>
public sealed class ComponentMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Component, ComponentResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<Guid, GetComponentByIdQuery>().Map(dest => dest.ComponentId, src => src);

        config
            .NewConfig<CreateComponentRequest, CreateComponentCommand>()
            .Map(dest => dest.Name, src => src.Name);

        config.NewConfig<Guid, DeleteComponentCommand>().Map(dest => dest.ComponentId, src => src);

        config
            .NewConfig<(Guid ComponentId, UpdateComponentRequest Request), UpdateComponentCommand>()
            .Map(dest => dest.ComponentId, src => src.ComponentId)
            .Map(dest => dest.Name, src => src.Request.Name);
    }
}
