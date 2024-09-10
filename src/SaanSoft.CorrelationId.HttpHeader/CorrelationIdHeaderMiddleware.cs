using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SaanSoft.CorrelationId.HttpHeader;

public class CorrelationIdHeaderMiddleware(RequestDelegate next)
{
    private readonly static Dictionary<string, Func<string?, string?>> HeaderExtractions =
        new()
        {
            { "traceparent", ExtractTraceParentTraceId },
            { "x-correlation-id", ExtractValue },
            { "correlation-id", ExtractValue },
            { "correlationid", ExtractValue }
        };

    public async Task InvokeAsync(HttpContext context, ICorrelationIdProvider correlationIdProvider)
    {
        var correlationId = GetCorrelationId(context, correlationIdProvider);

        // Call the next delegate/middleware in the pipeline.
        await next(context);
    }

    private static string GetCorrelationId(HttpContext context, ICorrelationIdProvider correlationIdProvider)
    {
        foreach (var (key, extractFunc) in HeaderExtractions)
        {
            if (context.Request.Headers.TryGetValue(key, out var foundHeader))
            {
                var correlationId = extractFunc(foundHeader.FirstOrDefault());
                if (!string.IsNullOrWhiteSpace(correlationId))
                {
                    correlationIdProvider.Set(correlationId);
                    return correlationId;
                }
            }
        }

        // return default if nothing else matches
        return correlationIdProvider.Get();
    }

    /// <summary>
    /// traceparent header is in the format: [VERSION]-[TRACE_ID]-[PARENT_ID]-[TRACE_FLAGS]
    ///
    /// "trace-id" is the equivalent of the correlationId
    ///
    /// For more details <see href="https://www.w3.org/TR/trace-context-2/#traceparent-header"/>
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    private static string? ExtractTraceParentTraceId(string? val)
    {
        if (string.IsNullOrWhiteSpace(val)) return null;

        var parts = val.Split("-");
        return parts.Length >= 2
            // currently there is only version="00" - where the second item is the "trace-id"
            ? parts[1].Trim()
            : null;
    }

    private static string? ExtractValue(string? val)
        => !string.IsNullOrWhiteSpace(val) ? val.Trim() : null;
}

public static class CorrelationIdHeaderMiddlewarextensions
{
    public static IApplicationBuilder UseCorrelationIdHeaderMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<CorrelationIdHeaderMiddleware>();
}
