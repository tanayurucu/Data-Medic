namespace DataMedic.Contracts.DeviceGroups;

public record DeviceGroupResponse(Guid Id, string Name, DateTime CreatedOnUtc, DateTime? ModifiedOnUtc);