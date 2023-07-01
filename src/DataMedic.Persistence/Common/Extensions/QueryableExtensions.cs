using DataMedic.Application.Common.Models;

using Microsoft.EntityFrameworkCore;

namespace DataMedic.Persistence.Common.Extensions;

public static class QueryableExtensions
{
    public static Paged<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new Paged<T>(items, count, pageNumber, pageSize);
    }

    public static async Task<Paged<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new Paged<T>(items, count, pageNumber, pageSize);
    }
}