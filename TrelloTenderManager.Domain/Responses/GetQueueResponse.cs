using TrelloTenderManager.Domain.Models.Queue;

namespace TrelloTenderManager.Domain.Responses;

/// <summary>
/// Represents the response for getting the queue.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GetQueueResponse"/> class.
/// </remarks>
/// <param name="csvQueueItems">The list of CSV queue items.</param>
public class GetQueueResponse(List<CsvQueueDto>? csvQueueItems)
{

    /// <summary>
    /// Gets or sets the list of CSV queue items.
    /// </summary>
    public List<CsvQueueDto>? CsvQueueItems { get; set; } = csvQueueItems;
}
