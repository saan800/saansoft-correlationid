using Microsoft.AspNetCore.Builder;

namespace SaanSoft.CorrelationId.HttpHeader;

public static class CorrelationIdHeaderMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationIdHeaderMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<CorrelationIdHeaderMiddleware>();
}
