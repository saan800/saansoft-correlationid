using Microsoft.AspNetCore.Http;

namespace SaanSoft.CorrelationId.Web;

/// <summary>
/// Options on how to find the CorrelationId for each http request
/// The middleware will use the first valid value
/// </summary>
public class WebCorrelationIdOptions
{
    /// <summary>
    /// Evaluation functions to get the CorrelationId. The first match will be used.
    /// </summary>
    public IEnumerable<Func<HttpContext, string?>> Evaluators { get; set; } = [];

    /// <summary>
    /// Add the CorrelationId to the response header with this name.
    /// Provide null if you don't want to add the correlationId to a response header
    /// @default: X-Correlation-ID
    /// </summary>
    public string? ResponseHeaderName { get; set; } = "X-Correlation-ID";

    /// <summary>
    /// If the <see cref="ResponseHeaderName"/> already exists in the response, should it be overwritten
    /// @default: false
    /// </summary>
    public bool OverrideResponseHeader { get; set; }

    /// <summary>
    /// Begin the logger.BeginScope with the CorrelationId
    /// @default: true
    /// </summary>
    public bool AddCorrelationIdToLoggerScope { get; set; } = true;
}
