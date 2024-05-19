namespace TrelloTenderManager.Domain.Enums;

/// <summary>
/// Represents the status of a tender.
/// </summary>
public enum TenderStatus
{
    /// <summary>
    /// The status is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The tender is open.
    /// </summary>
    Open = 1,

    /// <summary>
    /// The tender is pending.
    /// </summary>
    Pending = 2,

    /// <summary>
    /// The tender is closed.
    /// </summary>
    Closed = 3
}
