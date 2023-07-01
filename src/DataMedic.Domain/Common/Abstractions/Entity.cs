namespace DataMedic.Domain.Common.Abstractions;

public interface IEntity { }
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    protected Entity(TId id)
    {
        Id = id;
    }

    protected Entity() { }

    public TId Id { get; protected set; }

    public static bool operator ==(Entity<TId> left, Entity<TId> right) => Equals(left, right);

    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !Equals(left, right);

    public override bool Equals(object? obj) => obj is Entity<TId> other && Id.Equals(other.Id);

    public bool Equals(Entity<TId>? other) => Equals((object?)other);

    public override int GetHashCode() => Id.GetHashCode();
}