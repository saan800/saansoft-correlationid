namespace SaanSoft.CorrelationId;

/// <summary>
/// If no correlationId is set, it will default to `Guid.NewGuid().ToString()` to ensure a value is always provided
/// </summary>
public class CorrelationIdProvider : ICorrelationIdProvider
{
    private string? _correlationId;

    public void Set(string correlationId)
    {
        if (string.IsNullOrWhiteSpace(correlationId)) return;

        correlationId = correlationId.Trim();
        var distinctChars = correlationId.Distinct();
        // ie "00000000"
        if (distinctChars.Count() == 1 && distinctChars.Contains('0')) return;
        // ie Guid.Empty
        if (distinctChars.Count() == 2 && distinctChars.Contains('0') && distinctChars.Contains('-')) return;

        _correlationId = correlationId;
    }

    public string Get()
    {
        if (string.IsNullOrWhiteSpace(_correlationId)) _correlationId = Guid.NewGuid().ToString();
        return _correlationId;
    }
}
