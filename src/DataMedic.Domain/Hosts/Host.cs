using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Interfaces;
using DataMedic.Domain.Hosts.ValueObjects;

namespace DataMedic.Domain.Hosts;

public sealed class Host : AggregateRoot<HostId>, IAuditableEntity, ISoftDeletableEntity
{
    public List<HostUri> Uris { get; private set; }
    public HostName Name { get; private set; }
    public HostType Type { get; private set; }
    public HostCredential Credential { get; private set; }
    public HostSslConfiguration SslConfiguration { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public DateTime? DeletedOnUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    private Host(
        HostId id,
        HostName name,
        HostType type,
        List<HostUri> uris,
        HostCredential credentials,
        HostSslConfiguration sslConfiguration
    )
        : base(id)
    {
        Name = name;
        Type = type;
        Uris = uris;
        Credential = credentials;
        SslConfiguration = sslConfiguration;
    }

    private Host() { }

    public static Host Create(
        HostName name,
        HostType type,
        List<HostUri> uriList,
        HostCredential credentials,
        HostSslConfiguration sslConfiguration
    ) => new(HostId.CreateUnique(), name, type, uriList, credentials, sslConfiguration);

    public void SetName(HostName name) => Name = name;

    public void SetType(HostType type) => Type = type;

    public void SetUriList(List<HostUri> uriList) => Uris = uriList;

    public void SetCredential(HostCredential credential) => Credential = credential;

    public void SetSslConfiguration(HostSslConfiguration sslConfiguration) =>
        SslConfiguration = sslConfiguration;
}
