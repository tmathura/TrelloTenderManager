using TrelloTenderManager.Domain.DataAccessObjects;

namespace TrelloTenderManager.Core.Interfaces;

public interface ICsvQueueBl
{
    /// <summary>
    /// Creates a new CSV queue with the specified content.
    /// </summary>
    /// <param name="csvFileContent">The content of the CSV file.</param>
    /// <returns>The ID of the created CSV queue.</returns>
    Task<int> CreateCsvQueue(string csvFileContent);

    /// <summary>
    /// Reads the first unprocessed CSV queue.
    /// </summary>
    /// <returns>The first unprocessed CSV queue, or null if no unprocessed queue is found.</returns>
    Task<CsvQueueDao?> ReadFirstUnprocessedCsvQueue();

    /// <summary>
    /// Updates the specified CSV queue.
    /// </summary>
    /// <param name="csvQueue">The CSV queue to update.</param>
    /// <returns>The number of affected rows.</returns>
    Task<int> UpdateCsvQueue(CsvQueueDao csvQueue);
}