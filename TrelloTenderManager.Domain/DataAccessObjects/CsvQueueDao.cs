using SQLite;

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
    /// Gets or sets the content of the CSV.
    /// </summary>
    public string? CsvContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the CSV has been processed.
    /// </summary>
    public bool IsProcessed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the CSV processing failed.
    /// </summary>
    public bool FailedProcess { get; set; }

    /// <summary>
    /// Gets or sets the creation date and time of the CSV queue.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date and time of the CSV queue.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
