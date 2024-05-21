using RestSharp;

namespace TrelloTenderManager.WebApp.Services.Wrappers.Interfaces;

/// <summary>
/// Represents a wrapper for the RestSharp RestClient.
/// </summary>
public interface IRestClientWrapper
{
    /// <summary>
    /// Gets the RestClient instance.
    /// </summary>
    IRestClient RestClient { get; }
}
