using AutoFixture.Xunit2;

namespace SaanSoft.Tests.CorrelationId;

public class SimpleCorrelationIdProviderTests
{
    private readonly SimpleCorrelationIdProvider _provider = new();

    [Fact]
    public void Get_returns_non_empty_string()
    {
        var result = _provider.Get();

        result.Should().NotBeNullOrWhiteSpace();
        result.Length.Should().Be(36); // length of a guid
    }

    [Theory]
    [InlineAutoData]
    public void Set_and_Get_returns_given_value(string val)
    {
        _provider.Set(val);

        var result = _provider.Get();
        result.Should().Be(val);
    }
}
