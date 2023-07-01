using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Interfaces;
using DataMedic.Domain.OperatingSystems.ValueObjects;

namespace DataMedic.Domain.OperatingSystems;

public sealed class OperatingSystem
    : AggregateRoot<OperatingSystemId>,
        IAuditableEntity,
        ISoftDeletableEntity
{
    public OperatingSystemName Name { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public DateTime? DeletedOnUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    private OperatingSystem(OperatingSystemId id, OperatingSystemName name)
        : base(id) => Name = name;

    private OperatingSystem() { }

    public static OperatingSystem Create(OperatingSystemName name) =>
        new(OperatingSystemId.CreateUnique(), name);

    public void SetName(OperatingSystemName name) => Name = name;
}