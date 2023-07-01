using System.Diagnostics;
using System.Text.Json;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DataMedic.Application.Common.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TRequest>
    where TResponse : IErrorOr
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var traceId =
            Activity.Current?.Id
            ?? _httpContextAccessor.HttpContext?.TraceIdentifier
            ?? string.Empty;
        try
        {
            var result = await next();

            if (result.IsError)
            {
                _logger.LogError(
                    "Request failed {@RequestName} {@Request} {@Errors} {@TraceId}",
                    typeof(TRequest).Name,
                    JsonSerializer.Serialize(request),
                    JsonSerializer.Serialize(result.Errors),
                    traceId
                );
            }

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception occurred while handling request {@RequestName} {@Request} {@Errors} {@TraceId}",
                typeof(TRequest).Name,
                JsonSerializer.Serialize(request),
                JsonSerializer.Serialize(e),
                traceId
            );
            throw;
        }
    }
}
