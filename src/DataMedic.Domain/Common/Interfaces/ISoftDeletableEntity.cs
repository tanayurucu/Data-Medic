namespace DataMedic.Domain.Common.Interfaces;

public interface ISoftDeletableEntity
{
    DateTime? DeletedOnUtc { get; }
    bool IsDeleted { get; }
}