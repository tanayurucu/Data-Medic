using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.DeviceGroups.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public sealed class DeviceGroupConfiguration : IEntityTypeConfiguration<DeviceGroup>
{
    public void Configure(EntityTypeBuilder<DeviceGroup> builder)
    {
        builder.ToTable(TableNames.DeviceGroups);

        builder.HasKey(deviceGroup => deviceGroup.Id);

        builder
            .Property(deviceGroup => deviceGroup.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => DeviceGroupId.Create(value));

        builder
            .Property(deviceGroup => deviceGroup.Name)
            .HasConversion(name => name.Value, value => DeviceGroupName.Create(value).Value)
            .HasMaxLength(DeviceGroupName.MaxLength)
            .IsRequired();

        builder.Property(deviceGroup => deviceGroup.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(deviceGroup => !deviceGroup.IsDeleted);
    }
}
