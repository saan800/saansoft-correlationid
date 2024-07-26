namespace SaanSoft.CorrelationId;

public interface ICorrelationIdProvider
{
    /// <summary>
    /// Set the correlationId to a specific value
    ///
    /// If the provided value is not valid (eg blank string or "000") then the value will not be set
    /// </summary>
    /// <param name="correlationId"></param>
    void Set(string correlationId);

    /// <summary>
    /// Get the correlationId
    /// </summary>
    /// <returns></returns>
    string Get();
}
