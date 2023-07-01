using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public sealed class PingSensorConfiguration : IEntityTypeConfiguration<PingSensor>
{
    public void Configure(EntityTypeBuilder<PingSensor> builder)
    {
        builder.ToTable(TableNames.PingSensors);

        builder.HasKey(pingSensor => pingSensor.Id);

        builder
            .Property(pingSensor => pingSensor.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => PingSensorId.Create(value));

        builder.Property(pingSensor => pingSensor.ScanPeriod);
    }
}
