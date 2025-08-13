using Microsoft.AspNetCore.Builder;

namespace SaanSoft.CorrelationId.Web;

public static class WebCorrelationIdMiddlewareExtensions
{
    /// <summary>
    /// Use the WebCorrelationIdMiddleware to configure the value supplied by ICorrelationIdProvider from HttpCntext. 
    /// It will use the default WebCorrelationIdOptions values.
    /// </summary>
// TODO: cref for ^^
// TODO: make options required, and this function ne up default 
    public static IApplicationBuilder UseWebCorrelationIdMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<WebCorrelationIdMiddleware>();

    /// <summary>
    /// Use the WebCorrelationIdMiddleware to configure the value supplied by ICorrelationIdProvider from HttpContext.
    /// </summary>
    public static IApplicationBuilder UseWebCorrelationIdMiddleware(this IApplicationBuilder builder, WebCorrelationIdOptions options)
        => builder.UseMiddleware<WebCorrelationIdMiddleware>(options);

}
