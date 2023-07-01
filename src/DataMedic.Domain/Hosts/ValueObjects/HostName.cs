using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.Hosts.ValueObjects;

public sealed class HostName : ValueObject
{
    public const int MaxLength = 64;
    public string Value { get; private set; }

    private HostName(string value) => Value = value;

    private HostName() { }

    public static ErrorOr<HostName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Errors.Host.HostNameRequired;
        }

        if (name.Length > MaxLength)
        {
            return Errors.Host.HostNameTooLong;
        }

        return new HostName(name);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator string(HostName v)
    {
        throw new NotImplementedException();
    }
}
