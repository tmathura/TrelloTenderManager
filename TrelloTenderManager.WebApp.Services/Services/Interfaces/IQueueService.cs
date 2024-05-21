using TrelloTenderManager.Domain.Models.Queue;
using TrelloTenderManager.Domain.Requests;

namespace TrelloTenderManager.WebApp.Services.Services.Interfaces;

/// <summary>
/// Represents a service for managing the queue.
/// </summary>
public interface IQueueService
{
    /// <summary>
    /// Queues the CSV data for processing.
    /// </summary>
    /// <param name="queueFromCsvRequest">The request containing the CSV data.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task QueueCsv(QueueFromCsvRequest queueFromCsvRequest);

    /// <summary>
    /// Retrieves the queue.
    /// </summary>
    /// <returns>A task representing the asynchronous operation that returns the list of CSV queue items.</returns>
    Task<List<CsvQueueDto>?> GetQueue();
}
