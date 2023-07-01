using DataMedic.Domain.Components;
using DataMedic.Domain.Components.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public sealed class ComponentConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder)
    {
        builder.ToTable(TableNames.Components);

        builder.HasKey(component => component.Id);

        builder
            .Property(component => component.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => ComponentId.Create(value));

        builder
            .Property(component => component.Name)
            .HasConversion(name => name.Value, value => ComponentName.Create(value).Value)
            .HasMaxLength(ComponentName.MaxLength);

        builder.Property(component => component.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(component => !component.IsDeleted);
    }
}
