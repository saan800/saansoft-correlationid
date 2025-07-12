namespace SaanSoft.CorrelationId;

/// <summary>
/// Provider so can access the correlationId from anywhere via dependency injection
/// </summary>
public interface ICorrelationIdProvider
{
    /// <summary>
    /// Set the correlationId to a specific value
    ///
    /// If the provided value is not valid (eg blank string or "000...") then the value will not be set
    /// </summary>
    /// <param name="correlationId"></param>
    void Set(string correlationId);

    /// <summary>
    /// Get the correlationId.
    /// If no correlationId is set yet, it will default to a random string to ensure a value is always provided
    /// </summary>
    string Get();
}
