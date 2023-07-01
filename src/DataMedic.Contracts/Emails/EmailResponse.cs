namespace DataMedic.Contracts.Emails;

public record EmailResponse(
    Guid Id,
    string Address,
    Guid DepartmentId,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc
);