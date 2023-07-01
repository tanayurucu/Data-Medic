using ErrorOr;

namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static class Common
    {
        public static class Pagination
        {
            public static Error InvalidPageNumber =>
                Error.Validation(
                    code: "Common.Pagination.InvalidPageNumber",
                    description: "Page number must be greater than zero."
                );

            public static Error InvalidPageSize =>
                Error.Validation(
                    code: "Common.Pagination.InvalidPageSize",
                    description: "Page size must be greater than zero."
                );
        }
    }
}