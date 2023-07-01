using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public sealed class NoderedSensorConfiguration : IEntityTypeConfiguration<NodeRedSensor>
{
    public void Configure(EntityTypeBuilder<NodeRedSensor> builder)
    {
        builder.ToTable(TableNames.NodeRedSensors);
        builder.HasKey(ns => ns.Id);
        builder.Property(ns => ns.Id).ValueGeneratedNever()
            .HasConversion(id => id.Value, value => NodeRedSensorId.Create(value));

        builder.Property(ns => ns.FlowId);
        builder.Property(ns => ns.LastErrorLog);

    }
}