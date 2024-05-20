using log4net;
using Microsoft.Extensions.Configuration;
using SQLite;
using System.Data.Common;
using System.Linq.Expressions;
using TrelloTenderManager.Domain.DataAccessObjects;
using TrelloTenderManager.Domain.Exceptions;
using TrelloTenderManager.Infrastructure.Interfaces;

namespace TrelloTenderManager.Infrastructure.Implementations;

/// <summary>
/// Represents a data access layer for CSV queue operations.
/// </summary>
public class CsvQueueDal : ICsvQueueDal
{
    private readonly SQLiteAsyncConnection _database;
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    /// <summary>
    /// Initializes a new instance of the <see cref="CsvQueueDal"/> class.
    /// </summary>
    /// <param name="configuration">The configuration object.</param>
    public CsvQueueDal(IConfiguration configuration)
    {
        var databaseName = configuration["Database:Name"];

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new AppSettingsException("Error getting database name string from configuration.");
        }

        var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, databaseName);

        try
        {
            _logger.Info("Set up database connection.");

            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<CsvQueueDao>().Wait();

            _logger.Info("Set up database connection completed.");
        }
        catch (Exception exception)
        {
            _logger.Error($"{exception.Message} - {exception.StackTrace}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<int> CreateCsvQueue(CsvQueueDao user)
    {
        try
        {
            _logger.Info("Start create insert of the CsvQueueDao into the database.");

            await _database.InsertAsync(user);

            _logger.Info("Completed create insert of CsvQueueDao into the database.");

            return user.Id;
        }
        catch (Exception exception)
        {
            _logger.Error($"{exception.Message} - {exception.StackTrace}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<CsvQueueDao>?> Read(Expression<Func<CsvQueueDao, bool>>? expression = null)
    {
        try
        {
            _logger.Info("Starting read of the CsvQueueDao from the database.");

            var csvQueueDaoTable = _database.Table<CsvQueueDao>();
            if (expression != null)
            {
                csvQueueDaoTable = csvQueueDaoTable.Where(expression);
            }

            var csvQueueDaos = await csvQueueDaoTable.ToListAsync();

            _logger.Info("Completed read of the CsvQueueDao from the database.");

            return csvQueueDaos;
        }
        catch (Exception exception)
        {
            _logger.Error($"{exception.Message} - {exception.StackTrace}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<CsvQueueDao?> ReadFirstUnprocessedCsvQueue()
    {
        try
        {
            _logger.Info("Starting read of the first unprocessed CsvQueueDao from the database.");

            var csvQueueDao = await _database.Table<CsvQueueDao>().Where(queueDao => !queueDao.IsProcessed).FirstOrDefaultAsync();

            _logger.Info("Completed read of the first unprocessed CsvQueueDao from the database completed.");

            return csvQueueDao;
        }
        catch (Exception exception)
        {
            _logger.Error($"{exception.Message} - {exception.StackTrace}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<int> UpdateCsvQueue(CsvQueueDao csvQueueDao)
    {
        try
        {
            _logger.Info("Start update insert of the CsvQueueDao into the database.");

            var result = await _database.UpdateAsync(csvQueueDao);

            _logger.Info("Completed update insert of CsvQueueDao into the database.");

            return result;
        }
        catch (Exception exception)
        {
            _logger.Error($"{exception.Message} - {exception.StackTrace}");
            throw;
        }
    }

    /// <summary>
    /// Disposes the resources used by the CSV queue data access layer.
    /// </summary>
    public void Dispose()
    {
        _database.CloseAsync().Wait();
    }
}
