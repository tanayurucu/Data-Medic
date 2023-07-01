using DataMedic.Application.Common.Models;
using DataMedic.Contracts.Common;

using Mapster;

namespace DataMedic.Presentation.Common.Mappings;

/// <summary>
/// Mappings for Common cases
/// </summary>
public sealed class CommonMappings : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.ForType(typeof(Paged<>), typeof(PagedResponse<>));
    }
}
