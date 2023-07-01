namespace DataMedic.Contracts.ControlSystems;

public record ControlSystemResponse(
    Guid Id,
    string Name,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc
);
