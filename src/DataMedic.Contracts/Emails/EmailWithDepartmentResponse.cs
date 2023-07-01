using DataMedic.Contracts.Departments;

namespace DataMedic.Contracts.Emails;

public record EmailWithDepartmentResponse(
    Guid Id,
    string Address,
    DepartmentResponse Department,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc
);