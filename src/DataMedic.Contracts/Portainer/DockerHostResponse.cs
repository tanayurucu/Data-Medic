namespace DataMedic.Contracts.Portainer;

public record DockerHostResponse(
    int Id,
    string Name,
    int GroupId);