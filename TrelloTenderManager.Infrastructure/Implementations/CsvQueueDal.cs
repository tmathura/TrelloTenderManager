using log4net;
using Microsoft.Extensions.Configuration;
using SQLite;
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
            await _database.InsertAsync(user);
            
            return user.Id;
        }
        catch (Exception exception)
        {
            _logger.Error($"{exception.Message} - {exception.StackTrace}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<CsvQueueDao>?> ReadCsvQueue(Expression<Func<CsvQueueDao, bool>>? expression = null)
    {
        try
        {
            var csvQueueDaoTable = _database.Table<CsvQueueDao>();
            if (expression != null)
            {
                csvQueueDaoTable = csvQueueDaoTable.Where(expression);
            }

            var csvQueueDaos = await csvQueueDaoTable.ToListAsync();
            
            return csvQueueDaos;
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
            var result = await _database.UpdateAsync(csvQueueDao);
            
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
