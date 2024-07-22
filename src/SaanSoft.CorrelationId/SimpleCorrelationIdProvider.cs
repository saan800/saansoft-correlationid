namespace SaanSoft.CorrelationId;

/// <summary>
/// If no correlationId is set, it will default to `Guid.NewGuid().ToString()` to ensure a value is always parovided.
/// </summary>
public class SimpleCorrelationIdProvider : ICorrelationIdProvider
{
    private string? _correlationId;

    public void Set(string correlationId)
    {
        if (!string.IsNullOrWhiteSpace(correlationId)) _correlationId = correlationId.Trim();
    }

    public string? Get()
    {
        if (string.IsNullOrWhiteSpace(_correlationId)) _correlationId = Guid.NewGuid().ToString();
        return _correlationId;
    }
}
