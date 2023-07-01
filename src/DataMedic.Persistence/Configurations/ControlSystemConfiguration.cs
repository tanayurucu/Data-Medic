using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.ControlSystems.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public sealed class ControlSystemConfiguration : IEntityTypeConfiguration<ControlSystem>
{
    public void Configure(EntityTypeBuilder<ControlSystem> builder)
    {
        builder.ToTable(TableNames.ControlSystems);

        builder.HasKey(controlSystem => controlSystem.Id);

        builder
            .Property(controlSystem => controlSystem.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => ControlSystemId.Create(value));

        builder
            .Property(controlSystem => controlSystem.Name)
            .HasConversion(name => name.Value, value => ControlSystemName.Create(value).Value)
            .HasMaxLength(ControlSystemName.MaxLength)
            .IsRequired();

        builder.Property(controlSystem => controlSystem.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(controlSystem => !controlSystem.IsDeleted);
    }
}
