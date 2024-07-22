namespace SaanSoft.CorrelationId;

public interface ICorrelationIdProvider
{
    /// <summary>
    /// Set the correlationId to a specific value
    /// </summary>
    /// <param name="correlationId"></param>
    void Set(string correlationId);

    /// <summary>
    /// Get the correlationId
    /// </summary>
    /// <returns></returns>
    string? Get();
}
