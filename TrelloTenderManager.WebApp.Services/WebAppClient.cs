using TrelloTenderManager.WebApp.Services.Services.Implementations;
using TrelloTenderManager.WebApp.Services.Services.Interfaces;
using TrelloTenderManager.WebApp.Services.Wrappers.Interfaces;

namespace TrelloTenderManager.WebApp.Services;

/// <summary>
/// Represents a client for the web application.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WebAppClient"/> class.
/// </remarks>
/// <param name="restClientWrapper">The REST client wrapper.</param>
public class WebAppClient(IRestClientWrapper restClientWrapper) : IWebAppClient
{
    /// <inheritdoc />
    public ICardService CardService { get; } = new CardService(restClientWrapper.RestClient);

    /// <inheritdoc />
    public IQueueService QueueService { get; } = new QueueService(restClientWrapper.RestClient);
}
