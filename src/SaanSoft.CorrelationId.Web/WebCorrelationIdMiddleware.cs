using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SaanSoft.CorrelationId.Web;

/// <summary>
/// Get the CorrelationId for each http request (uses the first valid match from <see cref="WebCorrelationIdOptions.Evaluators"/>)
/// If can't find a CorrelationId from the Evaluators, it will default to a unique random string
/// Sets the CorrelationId on the <see cref="ICorrelationIdProvider"/>
/// </summary>
public class WebCorrelationIdMiddleware(RequestDelegate next, WebCorrelationIdOptions options)
{
    public async Task InvokeAsync(HttpContext httpContext, ICorrelationIdProvider correlationIdProvider)
    {
        // try evaluators to find a correlationId
        string? correlationId = options.Evaluators
            .Select(evaluator => evaluator.Invoke(httpContext))
            .FirstOrDefault(result => result.IsValidCorrelationId());

        // still nothing - just use guid generated value
        if (string.IsNullOrWhiteSpace(correlationId) || !correlationId.IsValidCorrelationId())
        {
            correlationId = Guid.NewGuid().ToString("N"); // N: removes "-" from guid
        }

        correlationIdProvider.Set(correlationId);

        if (!string.IsNullOrWhiteSpace(options.ResponseHeaderName))
        {
            httpContext.Response.OnStarting(() =>
            {
                // don't overwrite an existing correlationId, unless specifically configured to do so
                if (options.OverrideResponseHeader ||
                    !httpContext.Response.Headers.ContainsKey(options.ResponseHeaderName))
                {
                    httpContext.Response.Headers[options.ResponseHeaderName] = correlationId;
                }
                return Task.CompletedTask;
            });
        }

        ILogger? logger = null;
        if (options.AddCorrelationIdToLoggerScope)
        {
            var loggerFactory = httpContext.RequestServices.GetService<ILoggerFactory>();
            logger = loggerFactory?.CreateLogger<WebCorrelationIdMiddleware>();
        }

        if (logger != null)
        {
            using (logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
            {
                logger.LogDebug("Started request with CorrelationId {CorrelationId}", correlationId);
                await next(httpContext);
            }
        }
        else
        {
            await next(httpContext);
        }
    }
}

