using TrelloTenderManager.WebApp.Services.Services.Interfaces;

namespace TrelloTenderManager.WebApp.Services;

/// <summary>
/// Represents a client for the web application.
/// </summary>
public interface IWebAppClient
{
    /// <summary>
    /// Gets the service for managing cards.
    /// </summary>
    ICardService CardService { get; }

    /// <summary>
    /// Gets the service for managing queues.
    /// </summary>
    IQueueService QueueService { get; }
}
