using Bogus;

namespace DataMedic.Domain.Tests;

public static class FakerExtensions
{
    public static Faker<T> UsePrivateConstructor<T>(this Faker<T> faker)
        where T : class =>
        faker.CustomInstantiator(
            _ =>
                Activator.CreateInstance(typeof(T), nonPublic: true) is T t
                    ? t
                    : throw new InvalidCastException()
        );
}
