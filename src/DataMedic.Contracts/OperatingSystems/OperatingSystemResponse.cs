namespace DataMedic.Contracts.OperatingSystems;

public record OperatingSystemResponse(Guid Id, string Name, DateTime CreatedOnUtc, DateTime? ModifiedOnUtc);