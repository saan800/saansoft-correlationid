using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using SaanSoft.CorrelationId.Web;

namespace SaanSoft.Tests.CorrelationId.Web;

public class EvaluatorTests
{
    [Fact]
    public void UseTraceParentHeader_no_header_should_return_null()
    {
        var httpContext = Setup(new SetupOptions());
        var evaluator = Evaluator.UseTraceParentHeader;

        var result = evaluator.Invoke(httpContext);

        result.Should().BeNull();
    }

    [Fact]
    public void UseTraceParentHeader_invalid_format_header_should_return_null()
    {
        var httpContext = Setup(new SetupOptions
        {
            HeaderName = "traceparent",
            HeaderValue = "blahblah"
        });
        var evaluator = Evaluator.UseTraceParentHeader;

        var result = evaluator.Invoke(httpContext);

        result.Should().BeNull();
    }

    /// <summary>
    /// W#C traceparent header format - [VERSION]-[TRACE_ID]-[PARENT_ID]-[TRACE_FLAGS]
    /// </summary>
    [Theory]
    [InlineAutoData("traceparent")]
    [InlineAutoData("TraceParent")]
    [InlineAutoData("TRACEPARENT")]
    public void UseTraceParentHeader_valid_header_should_return_traceId(string headerName, string traceId, string parentId, string traceFlags)
    {
        // clean up generated values
        traceId = traceId.Replace("-", "");
        parentId = parentId.Replace("-", "");
        traceFlags = traceFlags.Replace("-", "");

        var httpContext = Setup(new SetupOptions
        {
            HeaderName = headerName,
            HeaderValue = $"00-{traceId}-{parentId}-{traceFlags}"
        });
        var evaluator = Evaluator.UseTraceParentHeader;

        var result = evaluator.Invoke(httpContext);

        result.Should().Be(traceId);
    }

    [Fact]
    public void UseHeader_with_no_header_in_context_should_return_null()
    {
        var httpContext = Setup(new SetupOptions());
        var evaluator = Evaluator.UseHeader("my-custom-header");

        var result = evaluator.Invoke(httpContext);

        result.Should().BeNull();
    }

    [Theory]
    [InlineAutoData("my-custom-header")]
    [InlineAutoData("My-Custom-Header")]
    [InlineAutoData("MY-CUSTOM-HEADER")]
    public void UseHeader_should_return_value(string headerName, string headerValue)
    {
        var httpContext = Setup(new SetupOptions
        {
            HeaderName = headerName,
            HeaderValue = headerValue
        });
        var evaluator = Evaluator.UseHeader(headerName.ToLower());

        var result = evaluator.Invoke(httpContext);

        result.Should().Be(headerValue);
    }

    [Theory]
    [InlineAutoData(null)]
    [InlineAutoData("")]
    [InlineAutoData("  ")]
    public void UseHttpContextTraceIdentifier_with_no_TraceIdentifier_in_context_should_return_null(string? traceIdentifier)
    {
        var httpContext = Setup(new SetupOptions
        {
            TraceIdentifier = traceIdentifier
        });
        var evaluator = Evaluator.UseHttpContextTraceIdentifier;

        var result = evaluator.Invoke(httpContext);

        result.Should().BeNull();
    }

    [Theory]
    [InlineAutoData]
    public void UseHttpContextTraceIdentifier_should_return_value(string? traceIdentifier)
    {
        var httpContext = Setup(new SetupOptions
        {
            TraceIdentifier = traceIdentifier
        });
        var evaluator = Evaluator.UseHttpContextTraceIdentifier;

        var result = evaluator.Invoke(httpContext);

        result.Should().Be(traceIdentifier);
    }

    private static HttpContext Setup(SetupOptions options)
    {
        var httpContext = A.Fake<HttpContext>();
        var request = A.Fake<HttpRequest>();
        var headers = new HeaderDictionary();
        if (!string.IsNullOrWhiteSpace(options.HeaderName))
        {
            headers[options.HeaderName] = options.HeaderValue;
        }

        A.CallTo(() => request.Headers).Returns(headers);
        A.CallTo(() => httpContext.Request).Returns(request);
        if (!string.IsNullOrWhiteSpace(options.TraceIdentifier))
        {
            A.CallTo(() => httpContext.TraceIdentifier).Returns(options.TraceIdentifier);
        }

        return httpContext;
    }

    private class SetupOptions
    {
        public string? HeaderName { get; set; }
        public string? HeaderValue { get; set; }
        public string? TraceIdentifier { get; set; }
    }
}
