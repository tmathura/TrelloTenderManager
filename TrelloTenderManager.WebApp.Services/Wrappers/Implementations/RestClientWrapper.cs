using Microsoft.Extensions.Configuration;
using RestSharp;
using TrelloTenderManager.Domain.Exceptions;
using TrelloTenderManager.WebApp.Services.Wrappers.Interfaces;

namespace TrelloTenderManager.WebApp.Services.Wrappers.Implementations;

/// <summary>
/// Wrapper class for RestClient.
/// </summary>
public class RestClientWrapper : IRestClientWrapper
{
    /// <inheritdoc />
    public IRestClient RestClient { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RestClientWrapper"/> class.
    /// </summary>
    /// <param name="configuration">The configuration object.</param>
    /// <exception cref="AppSettingsException">Thrown when error getting api base url from configuration.</exception>
    public RestClientWrapper(IConfiguration configuration)
    {
        var apiBaseUrl = configuration["WebApi:ApiBaseUrl"];

        if (string.IsNullOrWhiteSpace(apiBaseUrl))
        {
            throw new AppSettingsException("Error getting api base url from configuration.");
        }

        RestClient = new RestClient(apiBaseUrl);
    }
}
