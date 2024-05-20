using TrelloTenderManager.Domain.Enums;

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
    /// Gets or sets the filename of the CSV.
    /// </summary>
    public string? Filename { get; set; }
    
    /// <summary>
    /// Gets or sets the status of the CSV queue.
    /// </summary>
    public QueueStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the reason for the failure of the CSV processing.
    /// </summary>
    public string? FailedReason { get; set; }
}
