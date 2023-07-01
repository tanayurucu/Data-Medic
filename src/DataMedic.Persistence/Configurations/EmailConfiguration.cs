using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using DataMedic.Domain.Departments;
using DataMedic.Domain.Emails;
using DataMedic.Domain.Emails.ValueObjects;
using DataMedic.Persistence.Common.Constants;

namespace DataMedic.Persistence.Configurations;

public sealed class EmailConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.ToTable(TableNames.Emails);

        builder.HasKey(email => email.Id);

        builder
            .Property(email => email.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => EmailId.Create(value));

        builder
            .Property(email => email.Address)
            .HasConversion(address => address.Value, value => EmailAddress.Create(value).Value)
            .HasMaxLength(EmailAddress.MaxLength);

        builder
            .HasOne<Department>()
            .WithMany()
            .HasForeignKey(email => email.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}