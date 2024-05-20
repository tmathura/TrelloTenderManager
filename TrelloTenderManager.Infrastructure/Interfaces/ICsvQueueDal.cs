using System.Linq.Expressions;
using TrelloTenderManager.Domain.DataAccessObjects;

namespace TrelloTenderManager.Infrastructure.Interfaces;

/// <summary>
/// Represents the interface for interacting with the CSV queue data access layer.
/// </summary>
public interface ICsvQueueDal
{
    /// <summary>
    /// Creates a new CSV queue entry.
    /// </summary>
    /// <param name="user">The CSV queue data access object.</param>
    /// <returns>The number of affected rows.</returns>
    Task<int> CreateCsvQueue(CsvQueueDao user);

    /// <summary>
    /// Reads the CSV queue entries based on the specified expression.
    /// </summary>
    /// <param name="expression">The expression to filter the CSV queue entries.</param>
    /// <returns>The list of CSV queue data access objects.</returns>
    Task<List<CsvQueueDao>?> Read(Expression<Func<CsvQueueDao, bool>>? expression = null);

    /// <summary>
    /// Reads the first unprocessed CSV queue entry.
    /// </summary>
    /// <returns>The first unprocessed CSV queue data access object, or null if no unprocessed entry exists.</returns>
    Task<CsvQueueDao?> ReadFirstUnprocessedCsvQueue();

    /// <summary>
    /// Updates an existing CSV queue entry.
    /// </summary>
    /// <param name="user">The CSV queue data access object.</param>
    /// <returns>The number of affected rows.</returns>
    Task<int> UpdateCsvQueue(CsvQueueDao user);
}
