namespace DataMedic.Contracts.Sensors;

public sealed record SensorWithSensorDetailResponse(
    Guid Id,
    int Type,
    string Description,
    bool Status,
    bool IsActive,
    string StatusText,
    Guid HostId,
    DateTime? LastCheckOnUtc,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc,
    ISensorDetailResponse SensorDetail
);
