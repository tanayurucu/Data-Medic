using ErrorOr;

namespace DataMedic.Domain.Common.Errors;

public static partial class Errors
{
    public static List<Error> Combine(params IErrorOr[] errors) =>
        errors.Where(error => error?.IsError == true).SelectMany(error => error.Errors!).ToList();

    public static List<Error> Combine(params Error?[] errors) => errors.OfType<Error>().ToList();
}