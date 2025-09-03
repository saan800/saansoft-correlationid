using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SaanSoft.CorrelationId;
using SaanSoft.CorrelationId.Web;

namespace SaanSoft.Tests.CorrelationId.Web;

public class WebCorrelationIdMiddlewareTests
{
    private ICorrelationIdProvider _correlationIdProvider;
    private HttpClient _httpClient;

    [Fact]
    public async Task No_evaluators_should_return_guid_string()
    {
        Setup(new WebCorrelationIdOptions());
        await _httpClient.GetAsync("/");

        // Assert
        var result = _correlationIdProvider.Get();
        Guid.TryParse(result, out _).Should().BeTrue();
    }

    [Theory]
    [InlineAutoData]
    public async Task Options_UseHttpContextTractIdentifier_should_return_HttpContext_TraceIdentifier(
        string httpContextTraceIdentifier
    )
    {
        Setup(
            new WebCorrelationIdOptions
            {
                Evaluators = [Evaluator.UseHttpContextTraceIdentifier]
            },
            httpContextTraceIdentifier
        );
        await _httpClient.GetAsync("/");

        // Assert
        var result = _correlationIdProvider.Get();
        result.Should().Be(httpContextTraceIdentifier);
    }

    [Theory]
    [InlineAutoData]
    public async Task Options_UseHeader_should_return_value_from_header(string headerName, string headerValue)
    {
        Setup(
            new WebCorrelationIdOptions
            {
                Evaluators = [Evaluator.UseHeader(headerName)]
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
                services.AddLogging(c =>
                {
                    c.AddConsole();
                });
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
