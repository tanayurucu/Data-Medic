namespace DataMedic.Contracts.Portainer;

public record ContainerInfoResponse(
    string Id,
    string State,
    string Name);