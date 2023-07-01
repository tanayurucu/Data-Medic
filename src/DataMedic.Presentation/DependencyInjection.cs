using System.Text;
using DataMedic.Application.Common.Interfaces.Infrastructure;
using DataMedic.Contracts.Hosts;
using DataMedic.Contracts.Sensors;
using DataMedic.Domain.Hosts;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Presentation.Common.Constants;
using DataMedic.Presentation.Common.Errors;

using Mapster;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace DataMedic.Presentation;

/// <summary>
/// Registers services to IoC container
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers Presentation layer services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new CreateSensorDetailConverter());
                options.SerializerSettings.Converters.Add(new UpdateSensorDetailConverter());
            });

        services.AddEndpointsApiExplorer();

        services.AddSingleton<ProblemDetailsFactory, DataMedicProblemDetailsFactory>();

        services.AddMappings();

        services.AddHttpContextAccessor();

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        services.AddCors(
            options =>
                options.AddPolicy(
                    CorsPolicies.LocalhostCorsPolicy,
                    policy =>
                        policy
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                )
        );

        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(PresentationAssembly.Assembly);
        services.AddSingleton(config);

        var serviceProvider = services.BuildServiceProvider();
        var encryptionService = serviceProvider.GetRequiredService<IEncryptionService>();
        config
            .NewConfig<HostCredential, HostCredentialsResponse>()
            .Map(
                dest => dest.Credential,
                src => DecryptCredential(src, encryptionService)
            );

        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    private static string DecryptCredential(
        HostCredential credential,
        IEncryptionService encryptionService
    )
    {
        if (credential.Type != CredentialType.None && credential.EncryptedCredential.Any())
        {
            var decryptedCredential = encryptionService.Decrypt(
                credential.EncryptedCredential,
                credential.EncryptionIV
            );
            return Encoding.UTF8.GetString(decryptedCredential);
        }
        return string.Empty;
    }
}
