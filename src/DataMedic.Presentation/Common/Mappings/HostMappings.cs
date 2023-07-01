using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Cache;
using System.Diagnostics;
using DataMedic.Application.Hosts.Commands.CreateHost;
using DataMedic.Application.Hosts.Commands.DeleteHost;
using DataMedic.Application.Hosts.Commands.UpdateHost;
using DataMedic.Application.Hosts.Queries.GetAllHosts;
using DataMedic.Application.Hosts.Queries.GetHostById;
using DataMedic.Application.Hosts.Queries.GetHostsWithPagination;
using DataMedic.Contracts.Hosts;
using DataMedic.Domain.Hosts;

using Mapster;
using DataMedic.Application.Hosts.Queries.Portainer.GetPortainerHosts;
using DataMedic.Application.Hosts.Queries.Portainer.GetPortainerContainers;
using DataMedic.Application.Common.Models.Portainer;
using DataMedic.Contracts.Hosts.Portainer;
using ErrorOr;
using DataMedic.Application.Hosts.Queries.Kafka.GetKafkaTopics;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Application.Common.Interfaces.Infrastructure;

namespace DataMedic.Presentation.Common.Mappings;

/// <summary>
/// Mappings for Host
/// </summary>
public sealed class HostMappings : IRegister
{

    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Host, HostResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Uris, src => src.Uris.Select(uri => uri.Value))
            .Map(dest => dest.Credentials, src => src.Credential)
            .Map(
                dest => dest.SslConfiguration.Certificate,
                src =>
                    ConvertSslCertificateToResponse(
                        src.SslConfiguration.GetCertificate(),
                        src.SslConfiguration.Passphrase
                    )
            )
            .Map(
                dest => dest.SslConfiguration.CertificateAuthority,
                src =>
                    ConvertSslCertificateToResponse(
                        src.SslConfiguration.GetCertificateAuthority(),
                        string.Empty
                    )
            );

        config.NewConfig<GetHostsWithPaginationQueryParameters, GetHostsWithPaginationQuery>();

        config.NewConfig<GetAllHostsQueryParameters, GetAllHostsQuery>();

        config.NewConfig<CreateHostRequest, CreateHostCommand>();

        config
            .NewConfig<(Guid hostId, UpdateHostRequest request), UpdateHostCommand>()
            .Map(dest => dest.HostId, src => src.hostId)
            .Map(dest => dest.Credentials, src => src.request.Credentials)
            .Map(dest => dest.Name, src => src.request.Name)
            .Map(dest => dest.SslConfiguration, src => src.request.SslConfiguration)
            .Map(dest => dest.Type, src => src.request.Type)
            .Map(dest => dest.Uris, src => src.request.Uris);

        config.NewConfig<Guid, GetHostByIdQuery>().Map(dest => dest.HostId, src => src);

        config.NewConfig<Guid, DeleteHostCommand>().Map(dest => dest.HostId, src => src);

        config.NewConfig<Guid, GetPortainerHostsQuery>().Map(dest => dest.HostId, src => src);

        config
            .NewConfig<(Guid hostId, int portainerHostId), GetPortainerContainersQuery>()
            .Map(dest => dest.HostId, src => src.hostId)
            .Map(dest => dest.PortainerHostId, src => src.portainerHostId);

        config.NewConfig<PortainerHostInformation, PortainerHostInformationResponse>();

        config.NewConfig<PortainerContainerInformation, PortainerContainerInformationResponse>();

        config.NewConfig<Guid, GetKafkaTopicsQuery>().Map(dest => dest.HostId, src => src);
    }

    private static SslCertificateResponse? ConvertSslCertificateToResponse(
        ErrorOr<X509Certificate2> certificateOrError,
        string passphrase
    ) =>
        certificateOrError.IsError
            ? null
            : new SslCertificateResponse(
                certificateOrError.Value.SubjectName.Name,
                certificateOrError.Value.IssuerName.Name,
                certificateOrError.Value.NotAfter,
                certificateOrError.Value.RawData,
                passphrase
            );
}
