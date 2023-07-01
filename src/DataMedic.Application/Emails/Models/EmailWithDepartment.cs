using DataMedic.Domain.Departments;
using DataMedic.Domain.Emails.ValueObjects;

namespace DataMedic.Application.Emails.Models;

public sealed record EmailWithDepartment(
    EmailId Id,
    EmailAddress Address,
    Department Department,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc
);
