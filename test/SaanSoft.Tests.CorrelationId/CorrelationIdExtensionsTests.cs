using AutoFixture.Xunit2;

namespace SaanSoft.Tests.CorrelationId;

public class CorrelationIdExtensionsTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("000000")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void IsValidCorrelationId_invalid_value_should_return_false(string? val)
    {
        val.IsValidCorrelationId().Should().BeFalse();
    }

    [Theory]
    [InlineData("blah")]
    [InlineAutoData]
    public void IsValidCorrelationId_some_value_should_return_true(string val)
    {
        val.IsValidCorrelationId().Should().BeTrue();
    }

    [Fact]
    public void IsValidCorrelationId_guid_value_should_return_true()
    {
        Guid.NewGuid().ToString("N").IsValidCorrelationId().Should().BeTrue();
    }
}
