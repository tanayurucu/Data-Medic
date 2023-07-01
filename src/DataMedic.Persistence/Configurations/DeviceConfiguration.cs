using DataMedic.Domain.Components;
using DataMedic.Domain.ControlSystems;
using DataMedic.Domain.Departments;
using DataMedic.Domain.DeviceGroups;
using DataMedic.Domain.Devices;
using DataMedic.Domain.Devices.Entities;
using DataMedic.Domain.Devices.ValueObjects;
using DataMedic.Persistence.Common.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Persistence.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        ConfigureDevicesTable(builder);
        ConfigureDeviceComponentsTable(builder);
    }

    public static void ConfigureDevicesTable(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable(TableNames.Devices);

        builder.HasKey(device => device.Id);

        builder
            .Property(device => device.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => DeviceId.Create(value));

        builder
            .Property(device => device.Name)
            .HasConversion(deviceName => deviceName.Value, value => DeviceName.Create(value).Value)
            .HasMaxLength(DeviceName.MaxLength);

        builder.Property(device => device.Description);

        builder
            .Property(device => device.InventoryNumber)
            .HasConversion(
                inventoryNumber => inventoryNumber.Value,
                value => InventoryNumber.Create(value).Value
            )
            .HasMaxLength(InventoryNumber.MaxLength);

        builder
            .HasOne<DeviceGroup>()
            .WithMany()
            .HasForeignKey(device => device.DeviceGroupId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne<Department>()
            .WithMany()
            .HasForeignKey(device => device.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasQueryFilter(device => !device.IsDeleted);
    }

    public static void ConfigureDeviceComponentsTable(EntityTypeBuilder<Device> builder)
    {
        builder.OwnsMany(
            device => device.Components,
            deviceComponentBuilder =>
            {
                deviceComponentBuilder.ToTable(TableNames.DeviceComponents);

                deviceComponentBuilder.WithOwner().HasForeignKey(nameof(DeviceId));

                deviceComponentBuilder.HasKey(nameof(DeviceComponent.Id), nameof(DeviceId));
                deviceComponentBuilder
                    .Property(deviceComponent => deviceComponent.Id)
                    .HasColumnName(nameof(DeviceComponent.Id))
                    .ValueGeneratedNever()
                    .HasConversion(id => id.Value, value => DeviceComponentId.Create(value));

                deviceComponentBuilder
                    .Property(deviceComponent => deviceComponent.IpAddress)
                    .IsRequired()
                    .HasConversion(
                        ipAddress => ipAddress.Value,
                        value => IpAddress.Create(value).Value
                    )
                    .HasMaxLength(IpAddress.MaxLength);

                deviceComponentBuilder
                    .HasOne<Component>()
                    .WithMany()
                    .HasForeignKey(deviceComponent => deviceComponent.ComponentId)
                    .OnDelete(DeleteBehavior.NoAction);

                deviceComponentBuilder
                    .HasOne<OperatingSystem>()
                    .WithMany()
                    .HasForeignKey(deviceComponent => deviceComponent.OperatingSystemId)
                    .OnDelete(DeleteBehavior.NoAction);

                deviceComponentBuilder
                    .HasOne<ControlSystem>()
                    .WithMany()
                    .HasForeignKey(dc => dc.ControlSystemId)
                    .OnDelete(DeleteBehavior.NoAction);
            }
        );
        builder.Metadata
            .FindNavigation(nameof(Device.Components))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
