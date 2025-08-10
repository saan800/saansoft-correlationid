using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace SaanSoft.CorrelationId.Web;

public static class Evaluator
{
    /// <summary>
    /// The "traceparent" header is a commonly used W3C standard to track requests across systems.
    /// Its uses the format: `[VERSION]-[TRACE_ID]-[PARENT_ID]-[TRACE_FLAGS]`.
    ///
    /// `TRACE_ID` is the section used for the CorrelationId
    /// </summary>
    /// <remarks>
    /// For more details read https://www.w3.org/TR/trace-context-2/#traceparent-header
    /// </remarks>
    public static string? UseTraceParentHeader(this HttpContext httpContext)
        => httpContext.Request.Headers.TryGetValue("traceparent", out var val)
            ? ExtractW3CFormatTraceId(val)
            : null;

    /// <summary>
    /// Try to extract the correlationId from Activity.Current
    /// Its uses the format: `[VERSION]-[TRACE_ID]-[PARENT_ID]-[TRACE_FLAGS]`.
    ///
    /// `TRACE_ID` is the section used for the CorrelationId
    /// </summary>
    /// <remarks>
    /// For more details read https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activity?view=net-9.0
    /// </remarks>
    public static string? UseCurrentActivity(this HttpContext _)
        => ExtractW3CFormatTraceId(Activity.Current?.Id);

    /// <summary>
    /// Try to extract the correlationId from HttpContext.TraceIdentifier
    /// Format: {ConnectionId}:{Request number}
    /// </summary>
    /// <remarks>
    /// For more details read https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext.traceidentifier
    /// </remarks>
    public static string? UseHttpContextTraceIdentifier(this HttpContext httpContext)
    {
        var res = httpContext.TraceIdentifier.Trim();
        return res.IsValidCorrelationId() ? res : null;
    }

    /// <summary>
    /// Use the specified header value
    /// </summary>
    public static Func<HttpContext, string?> UseHeader(string headerName)
        => httpContext =>
        {
            if (!httpContext.Request.Headers.TryGetValue(headerName, out var val))
                return null;

            return CorrelationIdExtensions.IsValidCorrelationId(val)
                ? val.ToString()
                : null;
        };

    /// <summary>
    /// The "traceparent" header is a commonly used W3C standard to track requests across systems.
    /// Its uses the format: `[VERSION]-[TRACE_ID]-[PARENT_ID]-[TRACE_FLAGS]`.
    ///
    /// `TRACE_ID` is the section used for the CorrelationId
    /// </summary>
    /// <remarks>
    /// For more details read https://www.w3.org/TR/trace-context-2/#traceparent-header
    /// </remarks>
    private static string? ExtractW3CFormatTraceId(string? val)
    {
        var parts = (val ?? "").Split("-");
        if (parts.Length != 4) return null;

        // currently there is only version="00" - where the second item is the "TRACE_ID"
        var res = parts[1].Trim();
        return res.IsValidCorrelationId() ? res : null;
    }
}
