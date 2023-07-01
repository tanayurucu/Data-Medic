using DataMedic.Domain.Sensors.Entities;
using DataMedic.Domain.Sensors.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMedic.Persistence.Configurations;

public sealed class KafkaSensorConfiguration : IEntityTypeConfiguration<KafkaSensor>
{
    public void Configure(EntityTypeBuilder<KafkaSensor> builder)
    {
        builder.ToTable(TableNames.KafkaSensors);
        builder.HasKey(ks => ks.Id);
        builder.Property(ks => ks.Id).ValueGeneratedNever()
            .HasConversion(id => id.Value, value => KafkaSensorId.Create(value));
        builder.Property(kafkaSensor => kafkaSensor.TopicName)
            .HasConversion(topicName => topicName.Value, value => TopicName.Create(value).Value)
            .HasMaxLength(TopicName.MaxLength).IsRequired();
        builder.Property(kafkaSensor => kafkaSensor.TimeToLiveInSeconds)
            .HasConversion<long>();
        builder.Property(kafkaSensor => kafkaSensor.IdentifierKey)
            .HasMaxLength(64).IsRequired();
        builder.Property(kafkaSensor => kafkaSensor.IdentifierValue)
            .HasMaxLength(64).IsRequired();
    }
}