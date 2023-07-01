namespace DataMedic.Application.Common.Models.Portainer;

public record PortainerContainerStatus(
    string Id,
    string Name,
    bool Status,
    string StatusText,
    DateTime StartedAt
);
