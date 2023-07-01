using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataMedic.Persistence.Common.Converters;

internal sealed class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
        dateTime => DateOnly.FromDateTime(dateTime))
    {
    }
}