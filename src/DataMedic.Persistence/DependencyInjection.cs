using System.ComponentModel;
using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Sensors.Handlers;
using DataMedic.Persistence.Caching;
using DataMedic.Persistence.Common.Constants;
using DataMedic.Persistence.Common.Interceptors;
using DataMedic.Persistence.Repositories;

using Hangfire;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataMedic.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddInterceptors();

        services.AddDbContext<DataMedicDbContext>((serviceProvider, options) =>
        {
            var outboxInterceptor =
                serviceProvider.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            var updateAuditableEntitiesInterceptor =
                serviceProvider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();
            var updateSoftDeletableEntitiesInterceptor =
                serviceProvider.GetRequiredService<UpdateSoftDeletableEntitiesInterceptor>();
            options.UseSqlServer(configuration.GetConnectionString(ConnectionStringKeys.Database))
                .AddInterceptors(outboxInterceptor, updateAuditableEntitiesInterceptor, updateSoftDeletableEntitiesInterceptor);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IHandleKafkaSensorCreatedDomainEvent, HandleKafkaSensorCreatedDomainEvent>();
        services.AddScoped<IHandlePingSensorCreatedDomainEvent, HandlePingSensorCreatedDomainEvent>();
        services.AddScoped<IHandleDockerSensorCreatedDomainEvent, HandleDockerSensorCreatedDomainEvent>();
        services.AddScoped<IHandleNoderedSensorCreatedDomainEvent, HandleNoderedSensorCreatedDomainEvent>();
        services.AddSingleton<IHandleMqttSensorCreatedDomainEvent, HandleMqttSensorCreatedDomainEvent>();
        services.AddScoped<IHandleSensorStatusUpdatedDomainEvent, HandleSensorStatusUpdatedDomainEvent>();

        services.AddRepositories();

        services.AddCaching(configuration);
        var connectionString = configuration.GetConnectionString("HangfireConnection");
        //Hangfire
        // GlobalConfiguration.Configuration
        //     .UseSqlServerStorage(connectionString)
        //     .UseSerilogLogProvider();
        // Add Hangfire services to the DI container
        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(connectionString);
        });
        return services;
    }

    private static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddSingleton<UpdateSoftDeletableEntitiesInterceptor>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<IOperatingSystemRepository, OperatingSystemRepository>();
        services.AddScoped<IControlSystemRepository, ControlSystemRepository>();
        services.AddScoped<IDeviceGroupRepository, DeviceGroupRepository>();
        services.AddScoped<IHostRepository, HostRepository>();
        services.AddScoped<IComponentRepository, ComponentRepository>();
        services.AddScoped<IEmailRepository, EmailRepository>();

        return services;
    }

    private static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<CacheSettings>(configuration.GetSection(CacheSettings.SectionName));

        services.AddStackExchangeRedisCache(
            options =>
                options.Configuration = configuration.GetConnectionString(
                    ConnectionStringKeys.Redis
                )
        );

        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}
