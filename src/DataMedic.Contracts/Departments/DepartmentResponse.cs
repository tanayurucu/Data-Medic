namespace DataMedic.Contracts.Departments;

public record DepartmentResponse(Guid Id, string Name, DateTime CreatedOnUtc, DateTime? ModifiedOnUtc);