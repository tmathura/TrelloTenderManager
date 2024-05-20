using SQLite;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Domain.DataAccessObjects;

/// <summary>
/// Represents a data access object for CSV queue.
/// </summary>
public class CsvQueueDao
{
    /// <summary>
    /// Gets or sets the ID of the CSV queue.
    /// </summary>
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the filename of the CSV.
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Gets or sets the content of the CSV.
    /// </summary>
    public string? CsvContent { get; set; }

    /// <summary>
    /// Gets or sets the status of the CSV queue.
    /// </summary>
    public QueueStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the reason for the failure of the CSV processing.
    /// </summary>
    public string? FailedReason { get; set; }

    /// <summary>
    /// Gets or sets the creation date and time of the CSV queue.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date and time of the CSV queue.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
