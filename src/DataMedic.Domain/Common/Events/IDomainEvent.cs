using MediatR;

namespace DataMedic.Domain.Common.Events;

public interface IDomainEvent : INotification
{
    public Guid Id { get; }
}