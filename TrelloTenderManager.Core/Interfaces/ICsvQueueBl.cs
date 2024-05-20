using System.Linq.Expressions;
using TrelloTenderManager.Domain.DataAccessObjects;

namespace TrelloTenderManager.Core.Interfaces;

/// <summary>
/// Represents the business logic interface for CSV queue operations.
/// </summary>
public interface ICsvQueueBl
{
    /// <summary>
    /// Creates a new CSV queue with the specified content.
    /// </summary>
    /// <param name="filename">The name of the CSV file.</param>
    /// <param name="csvFileContent">The content of the CSV file.</param>
    /// <returns>The ID of the created CSV queue.</returns>
    Task<int> CreateCsvQueue(string filename, string csvFileContent);

    /// <summary>
    /// Reads the CSV queues based on the specified expression.
    /// </summary>
    /// <param name="expression">The expression to filter the CSV queues.</param>
    /// <returns>The list of CSV queues that match the specified expression.</returns>
    Task<List<CsvQueueDao>?> ReadCsvQueue(Expression<Func<CsvQueueDao, bool>>? expression = null);
    
    /// <summary>
    /// Updates the specified CSV queue.
    /// </summary>
    /// <param name="csvQueue">The CSV queue to update.</param>
    /// <returns>The number of affected rows.</returns>
    Task<int> UpdateCsvQueue(CsvQueueDao csvQueue);


    /// <summary>
    /// Queues the content of a CSV file for processing.
    /// </summary>
    /// <param name="filename">The name of the CSV file.</param>
    /// <param name="fileContent">The content of the CSV file.</param>
    Task QueueFromCsv(string filename, string fileContent);

    /// <summary>
    /// Processes the CSV queue.
    /// </summary>
    Task ProcessQueue();
}
