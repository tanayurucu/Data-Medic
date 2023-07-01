using DataMedic.Application.Common.Behaviors;
using DataMedic.Application.Sensors.Models;
using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

namespace DataMedic.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(
            configuration =>
                configuration
                    .RegisterServicesFromAssembly(ApplicationAssembly.Assembly)
                    .AddOpenBehavior(typeof(ValidationBehavior<,>))
                    .AddOpenBehavior(typeof(TransactionBehavior<,>))
                    .AddOpenBehavior(typeof(LoggingBehavior<,>))
        );

        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly, includeInternalTypes: true);
        services.AddSingleton<IDatabase>(provider =>
        {
            var connection = ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints = { "10.50.197.32:6380" },
                });
            return connection.GetDatabase();
        });
        return services;
    }
}
