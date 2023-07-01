namespace DataMedic.Contracts.Emails;

public record CreateEmailRequest(string Address, Guid DepartmentId);