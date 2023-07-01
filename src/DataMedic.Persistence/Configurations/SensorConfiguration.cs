using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors;
using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.ToTable(TableNames.Sensors);
        builder.HasKey(sensor => sensor.Id);

        builder
            .Property(sensor => sensor.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => SensorId.Create(value));

        builder
            .Property(sensor => sensor.DeviceComponentId)
            .ValueGeneratedNever()
            .HasConversion(
                deviceComponentId => deviceComponentId.Value,
                value => DeviceComponentId.Create(value)
            );

        builder.HasOne<Host>().WithMany().HasForeignKey(sensor => sensor.HostId);

        builder.OwnsOne(
            sensor => sensor.SensorDetail,
            sensorDetailBuilder =>
            {
                sensorDetailBuilder.Property(sensorDetail => sensorDetail.DetailId);
                sensorDetailBuilder
                    .Property(sensorDetail => sensorDetail.Type)
                    .HasConversion<int>();
            }
        );

        builder.Property(sensor => sensor.Status);
        builder.Property(sensor => sensor.StatusText).IsRequired(false);
        builder.Property(sensor => sensor.LastCheckOnUtc);
        builder.Property(sensor => sensor.Description).IsRequired(false);

        builder.HasQueryFilter(sensor => !sensor.IsDeleted);
    }
}
