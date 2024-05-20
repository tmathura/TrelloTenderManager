namespace TrelloTenderManager.Domain.Models.Queue;

/// <summary>
/// Represents a data access object for CSV queue.
/// </summary>
public class CsvQueueDto
{
    /// <summary>
    /// Gets or sets the ID of the CSV queue.
    /// </summary>
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
}
