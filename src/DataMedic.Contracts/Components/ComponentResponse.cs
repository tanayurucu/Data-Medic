namespace DataMedic.Contracts.Components;

public record ComponentResponse(
    Guid Id,
    string Name,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc
);
