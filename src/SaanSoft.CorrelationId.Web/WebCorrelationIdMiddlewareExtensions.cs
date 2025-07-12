using Microsoft.AspNetCore.Builder;

namespace SaanSoft.CorrelationId.Web;

public static class WebCorrelationIdMiddlewareExtensions
{
    /// <summary>
    /// Use the WebCorrelationIdMiddleware to configure the value supplied by ICorrelationIdProvider from web Http context and/por headers
    /// It will use the default WebCorrelationIdOptions values.
    /// </summary>
    public static IApplicationBuilder UseWebCorrelationIdMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<WebCorrelationIdMiddleware>();

    /// <summary>
    /// Use the WebCorrelationIdMiddleware to configure the value supplied by ICorrelationIdProvider from web Http context and/or headers
    /// </summary>
    public static IApplicationBuilder UseWebCorrelationIdMiddleware(this IApplicationBuilder builder, WebCorrelationIdOptions options)
        => builder.UseMiddleware<WebCorrelationIdMiddleware>(options);
}
