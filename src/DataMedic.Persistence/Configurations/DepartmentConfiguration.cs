using DataMedic.Domain.Departments;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable(TableNames.Departments);

        builder.HasKey(department => department.Id);

        builder
            .Property(department => department.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => DepartmentId.Create(value));

        builder
            .Property(department => department.Name)
            .HasConversion(name => name.Value, value => DepartmentName.Create(value).Value)
            .HasMaxLength(DepartmentName.MaxLength)
            .IsRequired();

        builder.Property(department => department.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(department => !department.IsDeleted);
    }
}
