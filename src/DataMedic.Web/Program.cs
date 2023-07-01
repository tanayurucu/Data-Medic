using DataMedic.Application;
using DataMedic.Infrastructure;
using DataMedic.Persistence;
using DataMedic.Presentation;
using DataMedic.Presentation.Common.Constants;
using DataMedic.Web;

using Microsoft.AspNetCore.Mvc.ApiExplorer;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddPersistence(builder.Configuration);

    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddApplication();

    builder.Services.AddPresentation();

    builder.Host.UseSerilog(
        (context, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
    );

    builder.Services.AddSwaggerGen();

    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

    builder.Services.AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
}

var app = builder.Build();

{
    if (app.Environment.IsDevelopment())
    {
        var apiVersionDescriptionProvider =
            app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (
                var groupName in apiVersionDescriptionProvider.ApiVersionDescriptions.Select(
                    description => description.GroupName
                )
            )
            {
                options.SwaggerEndpoint(
                    $"/swagger/{groupName}/swagger.json",
                    groupName.ToUpperInvariant()
                );
            }
        });
    }

    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();

    app.UseCors(CorsPolicies.LocalhostCorsPolicy);

    app.UseSerilogRequestLogging();

    app.UseAuthorization();

    app.UseStaticFiles();

    app.MapControllers();
}

app.Run();