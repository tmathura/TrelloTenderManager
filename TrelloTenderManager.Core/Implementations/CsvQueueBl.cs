using System.Linq.Expressions;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.DataAccessObjects;
using TrelloTenderManager.Infrastructure.Interfaces;

namespace TrelloTenderManager.Core.Implementations;

/// <summary>
/// Represents a CSV queue business logic implementation.
/// Initializes a new instance of the <see cref="CsvQueueBl"/> class.
/// </summary>
/// <param name="csvQueueDal">The CSV queue data access layer.</param>
public class CsvQueueBl(ICsvQueueDal csvQueueDal) : ICsvQueueBl
{
    /// <inheritdoc />
    public async Task<int> CreateCsvQueue(string csvFileContent)
    {
        var csvQueue = new CsvQueueDao
        {
            CsvContent = csvFileContent,
            IsProcessed = false,
            FailedProcess = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        return await csvQueueDal.CreateCsvQueue(csvQueue);
    }

    /// <inheritdoc />
    public async Task<List<CsvQueueDao>?> Read(Expression<Func<CsvQueueDao, bool>>? expression = null)
    {
        return await csvQueueDal.Read(expression);
    }

    /// <inheritdoc />
    public async Task<CsvQueueDao?> ReadFirstUnprocessedCsvQueue()
    {
        return await csvQueueDal.ReadFirstUnprocessedCsvQueue();
    }

    /// <inheritdoc />
    public async Task<int> UpdateCsvQueue(CsvQueueDao csvQueue)
    {
        csvQueue.UpdatedAt = DateTime.Now;

        return await csvQueueDal.UpdateCsvQueue(csvQueue);
    }
}
