using System.Runtime.InteropServices;
using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.Hosts.ValueObjects;

public sealed class HostUri : ValueObject
{
    public Uri Uri => new(Value);
    public string Value { get; private set; }

    private HostUri(string value) => Value = value;

    public static ErrorOr<HostUri> Create(string uri)
    {
        try
        {
            return new HostUri(new Uri(uri).OriginalString);
        }
        catch (Exception exception)
        {
            return Errors.Host.InvalidUri(exception.Message);
        }
    }

    public static ErrorOr<List<HostUri>> CreateMany(IEnumerable<string> uris)
    {
        if (!uris.Any())
        {
            return Errors.Host.UriRequired;
        }

        var hostUris = new List<HostUri>();
        foreach (var uri in uris)
        {
            var createHostUriResult = Create(uri);
            if (createHostUriResult.IsError)
            {
                return createHostUriResult.Errors;
            }

            if (hostUris.Contains(createHostUriResult.Value))
            {
                return Errors.Host.DuplicateUri;
            }

            hostUris.Add(createHostUriResult.Value);
        }

        return hostUris;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator HostUri(string value) => new(value);

    public static explicit operator string(HostUri value) => value.Value;
}
