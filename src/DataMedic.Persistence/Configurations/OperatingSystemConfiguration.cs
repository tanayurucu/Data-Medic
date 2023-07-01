using DataMedic.Domain.OperatingSystems;
using DataMedic.Domain.OperatingSystems.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Persistence.Configurations;

public sealed class OperatingSystemConfiguration : IEntityTypeConfiguration<OperatingSystem>
{
    public void Configure(EntityTypeBuilder<OperatingSystem> builder)
    {
        builder.ToTable(TableNames.OperatingSystems);

        builder.HasKey(operatingSystem => operatingSystem.Id);

        builder.Property(operatingSystem => operatingSystem.Id).ValueGeneratedNever()
            .HasConversion(id => id.Value, value => OperatingSystemId.Create(value));

        builder.Property(operatingSystem => operatingSystem.Name)
            .HasConversion(name => name.Value, value => OperatingSystemName.Create(value).Value)
            .HasMaxLength(OperatingSystemName.MaxLength).IsRequired();

        builder.Property(operatingSystem => operatingSystem.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(operatingSystem => !operatingSystem.IsDeleted);
    }
}