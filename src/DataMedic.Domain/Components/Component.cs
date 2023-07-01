using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Interfaces;
using DataMedic.Domain.Components.ValueObjects;

namespace DataMedic.Domain.Components;

public sealed class Component : AggregateRoot<ComponentId>, IAuditableEntity, ISoftDeletableEntity
{
    public ComponentName Name { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public DateTime? DeletedOnUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    private Component(ComponentId id, ComponentName name)
        : base(id) => Name = name;

    private Component() { }

    public static Component Create(ComponentName name) => new(ComponentId.CreateUnique(), name);

    public void SetName(ComponentName name) => Name = name;
}
