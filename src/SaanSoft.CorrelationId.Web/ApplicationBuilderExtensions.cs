using Microsoft.AspNetCore.Builder;

namespace SaanSoft.CorrelationId.Web;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Use the WebCorrelationIdMiddleware to configure the value supplied by ICorrelationIdProvider from HttpCntext.
    /// It will use the default <see cref="WebCorrelationIdOptions"/> values.
    /// </summary>
    public static IApplicationBuilder UseWebCorrelationIdMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<WebCorrelationIdMiddleware>(new WebCorrelationIdOptions());

    /// <summary>
    /// Use the WebCorrelationIdMiddleware to configure the value supplied by ICorrelationIdProvider from HttpContext.
    /// </summary>
    public static IApplicationBuilder UseWebCorrelationIdMiddleware(this IApplicationBuilder builder, WebCorrelationIdOptions options)
        => builder.UseMiddleware<WebCorrelationIdMiddleware>(options);

}
