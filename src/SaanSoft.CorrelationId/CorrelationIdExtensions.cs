namespace SaanSoft.CorrelationId;

public static class CorrelationIdExtensions
{
    /// <summary>
    /// Check if the value is a valid CorrelationId
    /// eg blank string, "000..." or Guid.Empty are not valid
    /// </summary>
    public static bool IsValidCorrelationId(this string? correlationId)
    {
        if (string.IsNullOrWhiteSpace(correlationId)) return false;

        correlationId = correlationId.Trim();
        var distinctChars = correlationId.Distinct().Except([' ']).ToList();

        // ie "000..."
        if (distinctChars.Count == 1 && distinctChars.Contains('0')) return false;
        // ie Guid.Empty
        if (distinctChars.Count == 2 && distinctChars.Contains('0') && distinctChars.Contains('-')) return false;

        return true;
    }
}
