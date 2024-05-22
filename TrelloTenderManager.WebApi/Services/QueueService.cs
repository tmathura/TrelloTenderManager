using log4net;
using TrelloTenderManager.Core.Queue.Interfaces;

namespace TrelloTenderManager.WebApi.Services;

/// <summary>
/// Represents a service for processing a queue.
/// </summary>
public class QueueService(ICsvQueueBl csvQueueBl) : BackgroundService
{
    private bool _isRunning;
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    /// <summary>
    /// Executes the queue service asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.Info("Queue service is starting.");

        _ = Task.Run(() => Process(cancellationToken), cancellationToken);

        return Task.CompletedTask;
    }

    private void Process(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_isRunning) continue;

            _isRunning = true;

            _ = Task.Factory.StartNew(async () =>
            {
                await csvQueueBl.ProcessQueue();

                _isRunning = false;
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }

    /// <summary>
    /// Stops the queue service asynchronously.
    /// </summary>
    /// <param name="stoppingToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.Info("Queue service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}