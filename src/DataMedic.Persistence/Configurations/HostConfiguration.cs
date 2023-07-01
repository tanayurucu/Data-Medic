using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public sealed class HostConfiguration : IEntityTypeConfiguration<Host>
{
    public void Configure(EntityTypeBuilder<Host> builder)
    {
        builder.ToTable(TableNames.Hosts);

        builder.HasKey(host => host.Id);

        builder
            .Property(host => host.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => HostId.Create(value));

        builder
            .Property(host => host.Name)
            .HasConversion(name => name.Value, value => HostName.Create(value).Value)
            .HasMaxLength(HostName.MaxLength);

        builder.Property(host => host.Type).HasConversion<int>();

        builder
            .Property(host => host.Uris)
            .HasConversion(
                uris => string.Join(";", uris.Select(uri => uri.Value)),
                value => HostUri.CreateMany(value.Split(";", StringSplitOptions.None)).Value,
                new ValueComparer<List<HostUri>>(
                    (collection1, collection2) =>
                        new HashSet<HostUri>(collection1!).SetEquals(
                            new HashSet<HostUri>(collection2!)
                        ),
                    collection =>
                        collection.Aggregate(
                            0,
                            (aggregate, value) => HashCode.Combine(aggregate, value.GetHashCode())
                        ),
                    collection => collection.ToList()
                )
            );

        builder.OwnsOne(
            host => host.Credential,
            credentialsBuilder =>
            {
                credentialsBuilder.Property(credential => credential.Type).HasConversion<int>();
                credentialsBuilder.Property(credential => credential.Username);
                credentialsBuilder.Property(credential => credential.EncryptedCredential);
                credentialsBuilder.Property(credential => credential.EncryptionIV);
            }
        );

        builder.OwnsOne(
            host => host.SslConfiguration,
            sslConfigurationBuilder =>
            {
                sslConfigurationBuilder.Property(
                    sslConfiguration => sslConfiguration.CertificateAuthorityData
                );
                sslConfigurationBuilder.Property(
                    sslConfiguration => sslConfiguration.CertificateData
                );
                sslConfigurationBuilder.Property(sslConfiguration => sslConfiguration.Passphrase);
            }
        );

        builder.HasQueryFilter(host => !host.IsDeleted);
    }
}
