using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Interfaces;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.Emails.ValueObjects;

namespace DataMedic.Domain.Emails;

public sealed class Email : AggregateRoot<EmailId>, IAuditableEntity
{
    private Email(EmailId id, EmailAddress mailAddress, DepartmentId departmentId)
        : base(id)
    {
        Address = mailAddress;
        DepartmentId = departmentId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Email"/> class.
    /// Required by EF Core.
    /// </summary>
#pragma warning disable CS8618
    private Email() { }
#pragma warning restore CS8618

    public EmailAddress Address { get; private set; }

    public DepartmentId DepartmentId { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ModifiedOnUtc { get; private set; }

    public static Email Create(EmailAddress emailAddress, DepartmentId departmentId) =>
        new(EmailId.CreateUnique(), emailAddress, departmentId);

    public void SetAddress(EmailAddress emailAddress) => Address = emailAddress;
}