using System.Diagnostics;
using System.Net;

using DataMedic.Presentation.Common.Constants;

using ErrorOr;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace DataMedic.Presentation.Common.Errors;

internal sealed class DataMedicProblemDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options;

    public DataMedicProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null,
        string? title = null,
        string? type = null, string? detail = null, string? instance = null)
    {
        statusCode ??= (int)HttpStatusCode.InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext,
        ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null,
        string? detail = null, string? instance = null)
    {
        if (modelStateDictionary is null)
        {
            throw new ArgumentNullException(nameof(modelStateDictionary));
        }

        statusCode ??= (int)HttpStatusCode.BadRequest;

        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        if (title is not null)
        {
            problemDetails.Title = title;
        }

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier ?? null;
        if (traceId is not null)
        {
            problemDetails.Extensions.Add(nameof(traceId), traceId);
        }

        if (httpContext.Items[HttpContextItemKeys.Errors] is List<Error> errors)
        {
            problemDetails.Extensions.Add("errorCodes", errors.Select(error => error.Code));
        }
    }
}