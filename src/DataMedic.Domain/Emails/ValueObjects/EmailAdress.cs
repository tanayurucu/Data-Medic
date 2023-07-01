using System.ComponentModel.DataAnnotations;

using ErrorOr;

using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

namespace DataMedic.Domain.Emails.ValueObjects;

public sealed class EmailAddress : ValueObject
{
    public const int MaxLength = 128;

    private EmailAddress(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailAddress"/> class.
    /// Required by EF Core.
    /// </summary>
#pragma warning disable CS8618
    private EmailAddress() { }
#pragma warning restore CS8618

    public string Value { get; private set; }

    public static ErrorOr<EmailAddress> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.Email.Address.Empty;
        }

        if (value.Length > MaxLength)
        {
            return Errors.Email.Address.TooLong;
        }

        if (!new EmailAddressAttribute().IsValid(value))
        {
            return Errors.Email.Address.Invalid;
        }

        return new EmailAddress(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator string(EmailAddress v) => v.Value;
}