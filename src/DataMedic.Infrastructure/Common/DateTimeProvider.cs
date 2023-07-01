using DataMedic.Application.Common.Interfaces.Infrastructure;

namespace DataMedic.Infrastructure.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.Now;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}