using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public sealed class DockerSensorConfiguration : IEntityTypeConfiguration<DockerSensor>
{
    public void Configure(EntityTypeBuilder<DockerSensor> builder)
    {
        builder.ToTable(TableNames.DockerSensors);

        builder.HasKey(dockerSensor => dockerSensor.Id);

        builder
            .Property(dockerSensor => dockerSensor.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => DockerSensorId.Create(value));

        builder.Property(dockerSensor => dockerSensor.PortainerId);

        builder.Property(dockerSensor => dockerSensor.ContainerId);

        builder.Property(dockerSensor => dockerSensor.LastLog);
    }
}
