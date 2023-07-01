using DataMedic.Application;
using DataMedic.Infrastructure;
using DataMedic.Persistence;
using DataMedic.Worker.Jobs.KafkaScanner;
using DataMedic.Worker.Jobs.MqttScanner;
using DataMedic.Worker.Jobs.NoderedScanner;
using DataMedic.Worker.Jobs.Portainer;

using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;

using Microsoft.AspNetCore.Hosting;

using StackExchange.Redis;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        (builder, services) =>
        {
            services.AddHttpClient();
            services.AddApplication();
            services.AddPersistence(builder.Configuration);
            // services.AddSingleton<IEmailService,EmailService>();
            services.AddEmailService(builder.Configuration);
            services.AddInfrastructure(builder.Configuration);
            services.AddScoped<IKafkaScanService, KafkaScanService>();
            services.AddScoped<INoderedScanService, NoderedScanService>();
            services.AddScoped<IDockerScannerRepository, DockerScannerRepository>();
            services.AddScoped<IMqttRepository, MqttRepository>();
            services.AddSingleton<IDatabase>(_ =>
            {
                var connection = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                EndPoints = { "10.50.197.32:6380" },
            });
                return connection.GetDatabase();
            });
            services.AddHostedService<CheckKafkaStatus>();
            services.AddHostedService<DockerScannerJob>();
            // services.AddHostedService<CheckMqttStatus>();
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"),
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));
            services.AddHangfireServer();
        }
    )
    .UseConsoleLifetime()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.Configure(app =>
            {
                var options = new DashboardOptions()
                {
                    Authorization = new[] { new MyAuthorizationFilter() }
                };
                app.UseHangfireDashboard("/hangfire", options);
            }
        );
    })
    .Build();


host.Start();
host.WaitForShutdown();
public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}