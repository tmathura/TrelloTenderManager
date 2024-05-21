using TrelloTenderManager.Domain.Models.Queue;

namespace TrelloTenderManager.WebApp.Models;

/// <summary>
/// Represents the model for displaying the queue.
/// </summary>
public class ShowQueueModel
{
    /// <summary>
    /// Gets or sets the list of CSV queue items.
    /// </summary>
    public List<CsvQueueDto>? CsvQueueItems { get; set; }
}
