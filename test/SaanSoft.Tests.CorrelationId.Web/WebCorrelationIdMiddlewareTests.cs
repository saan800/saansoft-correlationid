using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SaanSoft.CorrelationId;
using SaanSoft.CorrelationId.Web;

namespace SaanSoft.Tests.CorrelationId.Web;

public class WebCorrelationIdMiddlewareTests
{
    private ICorrelationIdProvider _correlationIdProvider;
    private HttpClient _httpClient;

    [Fact]
    public async Task Options_all_disabled_should_return_guid_string()
    {
        Setup(new WebCorrelationIdOptions
        {
            UseHttpContext = false,
            UseTraceParentHeader = false
        });
        await _httpClient.GetAsync("/");

        // Assert
        var result = _correlationIdProvider.Get();
        Guid.TryParse(result, out _).Should().BeTrue();
    }

    [Theory]
    [InlineAutoData]
    public async Task Options_UseHttpContext_enabled_should_return_HttpContext_TraceIdentifier(string httpContextTraceIdentifier)
    {
        Setup(
            new WebCorrelationIdOptions
            {
                UseHttpContext = true,
                UseTraceParentHeader = false
            },
            httpContextTraceIdentifier
        );
        await _httpClient.GetAsync("/");

        // Assert
        var result = _correlationIdProvider.Get();
        result.Should().Be(httpContextTraceIdentifier);
    }

    /// <summary>
    /// W#C traceparent header format - [VERSION]-[TRACE_ID]-[PARENT_ID]-[TRACE_FLAGS]
    /// </summary>
    [Theory]
    [InlineAutoData]
    public async Task Options_UseTraceParentHeader_enabled_should_return_TraceId_from_w3c_header(string traceId, string parentId, string traceFlags)
    {
        Setup(
            new WebCorrelationIdOptions
            {
                UseHttpContext = false,
                UseTraceParentHeader = true
            }
        );

        // clean up generated values
        traceId = traceId.Replace("-", "");
        parentId = parentId.Replace("-", "");
        traceFlags = traceFlags.Replace("-", "");

        var request = new HttpRequestMessage(HttpMethod.Get, "/");
        request.Headers.Add("traceparent", $"00-{traceId}-{parentId}-{traceFlags}");
        await _httpClient.SendAsync(request);

        // Assert
        var result = _correlationIdProvider.Get();
        result.Should().Be(traceId);
    }

    [Theory]
    [InlineAutoData]
    public async Task Options_HeaderNames_supplied_should_return_header_value(string headerName, string headerValue)
    {
        Setup(
            new WebCorrelationIdOptions
            {
                UseHttpContext = false,
                UseTraceParentHeader = false,
                HeaderNames = [headerName]
            }
        );

        var request = new HttpRequestMessage(HttpMethod.Get, "/");
        request.Headers.Add(headerName, headerValue);
        await _httpClient.SendAsync(request);

        // Assert
        var result = _correlationIdProvider.Get();
        result.Should().Be(headerValue);
    }

    private void Setup(WebCorrelationIdOptions option, string? httpContextTraceIdentifier = null)
    {
        _correlationIdProvider = new MockCorrelationIdProvider();
        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddScoped<ICorrelationIdProvider>(_ => _correlationIdProvider);
            })
            .Configure(app =>
            {
                if (!string.IsNullOrWhiteSpace(httpContextTraceIdentifier))
                {
                    app.Use(async (context, next) =>
                    {
                        context.TraceIdentifier = httpContextTraceIdentifier;
                        await next();
                    });
                }
                app.UseWebCorrelationIdMiddleware(option);
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Success");
                });
            });

        var server = new TestServer(builder);

        _httpClient = server.CreateClient();
    }

    private class MockCorrelationIdProvider : ICorrelationIdProvider
    {
        private string? _correlationId;
        public void Set(string correlationId)
        {
            _correlationId = correlationId;
        }

        public string Get()
        {
            return _correlationId ?? "";
        }
    }
}
