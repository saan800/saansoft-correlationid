namespace SaanSoft.CorrelationId.Web;

/// <summary>
/// Options on how to find the CorrelationId for each http request
/// The middleware will use the first valid value
/// </summary>
public class WebCorrelationIdOptions
{
    /// <summary>
    /// First: Try to extract the correlationId from HttpContext (eg HttpContext.TraceIdentifier)
    ///
    /// @default: true;
    /// </summary>
    /// <remarks>
    /// For more details read https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext.traceidentifier
    /// </remarks>
    public bool UseHttpContext { get; set; } = true;

    /// <summary>
    /// Second: The traceparent header is a commonly used W3C standard to track requests across systems.
    /// Its in the format: `[VERSION]-[TRACE_ID]-[PARENT_ID]-[TRACE_FLAGS]`.
    ///
    /// `TRACE_ID` is the equivalent of the CorrelationId
    ///
    /// @default: true
    /// </summary>
    /// <remarks>
    /// For more details read https://www.w3.org/TR/trace-context-2/#traceparent-header
    /// </remarks>
    public bool UseTraceParentHeader { get; set; } = true;

    /// <summary>
    /// Finally: Check if a header exist on the http request and use that value as the CorrelationId
    /// </summary>
    /// <example>["x-correlation-id"]</example>
    public string[] HeaderNames { get; set; } = [];
}
