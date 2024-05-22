using log4net;
using System.Linq.Expressions;
using TrelloTenderManager.Core.Managers.Interfaces;
using TrelloTenderManager.Core.Queue.Interfaces;
using TrelloTenderManager.Domain.DataAccessObjects;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Infrastructure.Interfaces;

namespace TrelloTenderManager.Core.Queue.Implementations;

/// <summary>
/// Represents a CSV queue business logic implementation.
/// Initializes a new instance of the <see cref="CsvQueueBl"/> class.
/// </summary>
/// <param name="csvQueueDal">The CSV queue data access layer.</param>
public class CsvQueueBl(ICsvQueueDal csvQueueDal, ICardManager cardManager) : ICsvQueueBl
{
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    /// <inheritdoc />
    public async Task<int> CreateCsvQueue(string? filename, string? csvFileContent)
    {
        var csvQueue = new CsvQueueDao
        {
            Filename = filename,
            CsvContent = csvFileContent,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        return await csvQueueDal.Create(csvQueue);
    }

    /// <inheritdoc />
    public async Task<List<CsvQueueDao>?> ReadCsvQueue(Expression<Func<CsvQueueDao, bool>>? expression = null)
    {
        return await csvQueueDal.Read(expression);
    }

    /// <inheritdoc />
    public async Task<int> UpdateCsvQueue(CsvQueueDao csvQueue)
    {
        csvQueue.UpdatedAt = DateTime.Now;

        return await csvQueueDal.Update(csvQueue);
    }

    /// <inheritdoc />
    public async Task QueueFromCsv(string? filename, string? fileContent)
    {
        await CreateCsvQueue(filename, fileContent);
    }

    /// <inheritdoc />
    public async Task ProcessQueue()
    {
        var csvQueueItems = await ReadCsvQueue(expression => expression.Status == QueueStatus.Unprocessed);

        if (csvQueueItems is null) return;

        foreach (var csvQueueItem in csvQueueItems.Where(csvQueueItem => !string.IsNullOrWhiteSpace(csvQueueItem.CsvContent)))
        {
            try
            {
                csvQueueItem.Status = QueueStatus.Processing;

                await UpdateCsvQueue(csvQueueItem);

                await cardManager.ProcessFromCsv(csvQueueItem.CsvContent);

                csvQueueItem.Status = QueueStatus.Processed;

                await UpdateCsvQueue(csvQueueItem);
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");

                csvQueueItem.Status = QueueStatus.Failed;
                csvQueueItem.FailedReason = exception.Message;

                await UpdateCsvQueue(csvQueueItem);
            }
        }
    }
}
