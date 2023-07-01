namespace DataMedic.Contracts.Common;

public record PagedResponse<T>(
    int CurrentPage,
    int TotalPages,
    int PageSize,
    int TotalCount,
    bool HasPrevious,
    bool HasNext,
    IReadOnlyCollection<T> Data
);
