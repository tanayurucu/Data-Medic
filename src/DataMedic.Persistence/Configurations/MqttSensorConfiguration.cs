using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public sealed class MqttSensorConfiguration : IEntityTypeConfiguration<MqttSensor>
{
    public void Configure(EntityTypeBuilder<MqttSensor> builder)
    {
        builder.ToTable(TableNames.MqttSensors);
        builder.HasKey(ms => ms.Id);
        builder.Property(ms => ms.Id).ValueGeneratedNever()
            .HasConversion(id => id.Value, value => MqttSensorId.Create(value));

        builder.Property(mSensor => mSensor.TopicName)
            .HasConversion(topicName => topicName.Value, value => TopicName.Create(value).Value)
            .HasMaxLength(TopicName.MaxLength).IsRequired();
        builder.Property(ms => ms.TimeToLiveInSeconds)
            .HasConversion<long>();
    }
}