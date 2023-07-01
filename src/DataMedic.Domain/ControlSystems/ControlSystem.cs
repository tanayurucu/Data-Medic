using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Interfaces;
using DataMedic.Domain.ControlSystems.ValueObjects;

using ErrorOr;

namespace DataMedic.Domain.ControlSystems;

public sealed class ControlSystem
    : AggregateRoot<ControlSystemId>,
        IAuditableEntity,
        ISoftDeletableEntity
{
    public ControlSystemName Name { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public DateTime? DeletedOnUtc { get; private set; }
    public bool IsDeleted { get; private set; }

    private ControlSystem(ControlSystemId id, ControlSystemName name)
        : base(id) => Name = name;

    private ControlSystem() { }

    public static ControlSystem Create(ControlSystemName name) =>
        new(ControlSystemId.CreateUnique(), name);

    public void SetName(ControlSystemName name) => Name = name;

    public ErrorOr<Updated> SetName(string name)
    {
        var controlSystemName = ControlSystemName.Create(name);
        if (controlSystemName.IsError)
        {
            return controlSystemName.Errors;
        }
        SetName(controlSystemName.Value);
        return Result.Updated;
    }
}
