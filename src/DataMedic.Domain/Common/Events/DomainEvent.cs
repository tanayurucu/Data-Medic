namespace DataMedic.Domain.Common.Events;

public abstract record DomainEvent(Guid Id) : IDomainEvent;