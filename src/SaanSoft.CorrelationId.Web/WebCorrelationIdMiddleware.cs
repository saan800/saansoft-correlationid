using Microsoft.AspNetCore.Http;

namespace SaanSoft.CorrelationId.Web;

/// <summary>
/// Get the CorrelationId for each http request (uses the first valid match according to <see cref="WebCorrelationIdOptions"/>)
/// If can't find a CorrelationId from the http context or request details, it will default to a unique random string
/// Sets the CorrelationId on the <see cref="ICorrelationIdProvider"/>
/// </summary>
public class WebCorrelationIdMiddleware(RequestDelegate next, WebCorrelationIdOptions? options = null)
{
    private readonly WebCorrelationIdOptions _options = options ?? new WebCorrelationIdOptions();

    public async Task InvokeAsync(HttpContext context, ICorrelationIdProvider correlationIdProvider)
    {
        string? correlationId = null;
        if (_options.UseHttpContext) correlationId = GetCorrelationIdFromHttpContext(context);

        // no correlationId yet, check the various headers in order
        if (!correlationId.IsValidCorrelationId())
        {
            var headerExtractions = new Dictionary<string, Func<string?, string?>>();
            if (_options.UseTraceParentHeader)
            {
                headerExtractions.Add("traceparent", ExtractTraceParentTraceIdFromHeader);
            }
            foreach (var headerName in _options.HeaderNames.Distinct())
            {
                headerExtractions.Add(headerName, ExtractValue);
            }

            correlationId = GetCorrelationIdFromHeader(context, headerExtractions);
        }

        if (string.IsNullOrWhiteSpace(correlationId)) correlationId = Guid.NewGuid().ToString();
        correlationIdProvider.Set(correlationId);

        // Call the next delegate/middleware in the pipeline.
        await next(context);
    }

    /// <remarks>
    /// For more details read https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext.traceidentifier
    /// </remarks>
    private static string? GetCorrelationIdFromHttpContext(HttpContext context)
    {
        return context.TraceIdentifier.IsValidCorrelationId()
            ? context.TraceIdentifier
            : null;
    }

    /// <summary>
    /// Check supplied headers exist and extract the correlationId.
    /// Returns first valid match
    /// </summary>
    private static string? GetCorrelationIdFromHeader(HttpContext context, Dictionary<string, Func<string?, string?>> headerExtractions)
    {
        var headerKeys = context.Request.Headers.Keys;
        foreach (var (key, extractFunc) in headerExtractions)
        {
            var matchingHeaderKey =
                headerKeys.FirstOrDefault(x => string.Equals(x, key, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(matchingHeaderKey) && context.Request.Headers.TryGetValue(matchingHeaderKey, out var foundHeader))
            {
                var correlationId = extractFunc(foundHeader.FirstOrDefault());
                if (correlationId.IsValidCorrelationId())
                {
                    return correlationId;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// The traceparent header is a commonly used W3C standard to track requests across systems
    /// and uses the format: `[VERSION]-[TRACE_ID]-[PARENT_ID]-[TRACE_FLAGS]`.
    ///
    /// `TRACE_ID` is the equivalent of the CorrelationId
    /// </summary>
    /// <remarks>
    /// For more details read https://www.w3.org/TR/trace-context-2/#traceparent-header
    /// </remarks>
    private static string? ExtractTraceParentTraceIdFromHeader(string? val)
    {
        if (string.IsNullOrWhiteSpace(val)) return null;

        var parts = val.Split("-");
        return parts.Length >= 2
            // currently there is only version="00" - where the second item is the "trace-id"
            ? parts[1].Trim()
            : null;
    }

    /// <summary>
    /// Use the value of the header
    /// </summary>
    private static string? ExtractValue(string? val)
        => !string.IsNullOrWhiteSpace(val) ? val.Trim() : null;
}

