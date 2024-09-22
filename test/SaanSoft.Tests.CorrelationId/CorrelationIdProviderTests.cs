using AutoFixture.Xunit2;

namespace SaanSoft.Tests.CorrelationId;

public class CorrelationIdProviderTests
{
    private readonly CorrelationIdProvider _provider = new();

    [Fact]
    public void Get_returns_non_empty_string_by_default()
    {
        var result = _provider.Get();

        result.Should().NotBeNullOrWhiteSpace();
        result.Should().NotBe(Guid.Empty.ToString());
        Guid.TryParse(result, out _).Should().BeTrue();
    }

    [Theory]
    [InlineAutoData]
    public void Set_and_Get_returns_given_value(string val)
    {
        _provider.Set(val);

        var result = _provider.Get();
        result.Should().Be(val);
    }

    [Theory]
    [InlineAutoData]
    public void Setting_twice_only_does_not_reset_the_value_nce_set(string val1, string val2)
    {
        _provider.Set(val1);
        _provider.Set(val2);

        var result = _provider.Get();
        result.Should().Be(val1);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("000000")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void Set_and_Get_invalid_value_returns_default_guid_new(string val)
    {
        _provider.Set(val);

        var result = _provider.Get();
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().NotBe(val);
        result.Should().NotBe(Guid.Empty.ToString());
        Guid.TryParse(result, out _).Should().BeTrue();
    }
}
