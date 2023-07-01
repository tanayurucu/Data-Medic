using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Domain.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataMedic.Persistence.Common.Interceptors;

public sealed class UpdateSoftDeletableEntitiesInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateSoftDeletableEntitiesInterceptor(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        foreach (var entityEntry in dbContext.ChangeTracker.Entries<ISoftDeletableEntity>())
        {
            if (entityEntry.State == EntityState.Deleted)
            {
                entityEntry.Property(entity => entity.DeletedOnUtc).CurrentValue =
                    _dateTimeProvider.UtcNow;

                entityEntry.Property(entity => entity.IsDeleted).CurrentValue = true;

                entityEntry.State = EntityState.Modified;

                UpdateDeletedEntityEntryReferencesToUnchanged(entityEntry);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (entityEntry.References.Any())
        {
            foreach (var referenceEntry in entityEntry.References.Where(reference =>
                         reference.TargetEntry?.State == EntityState.Deleted))
            {
                referenceEntry.TargetEntry!.State = EntityState.Unchanged;
                UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
            }
        }
    }
}