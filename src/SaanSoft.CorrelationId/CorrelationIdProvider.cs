namespace SaanSoft.CorrelationId;

/// <inheritdoc/>
public class CorrelationIdProvider : ICorrelationIdProvider
{
    private string? _correlationId;

    /// <inheritdoc/>
    public void Set(string correlationId)
    {
        if (!correlationId.IsValidCorrelationId()) return;
        _correlationId = correlationId.Trim();
    }

    /// <inheritdoc/>
    public string Get()
    {
        if (!_correlationId.IsValidCorrelationId()) _correlationId = Guid.NewGuid().ToString("N");

        // ReSharper disable once NullableWarningSuppressionIsUsed
        return _correlationId!;
    }
}
