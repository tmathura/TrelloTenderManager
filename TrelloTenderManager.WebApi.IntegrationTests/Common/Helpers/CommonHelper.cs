using Microsoft.Extensions.Configuration;
using RestSharp;
using TrelloTenderManager.WebApi.IntegrationTests.Common.Models;
using Xunit.Abstractions;

namespace TrelloTenderManager.WebApi.IntegrationTests.Common.Helpers;

/// <summary>
/// Helper class for common operations in integration tests.
/// </summary>
public class CommonHelper
{
    /// <summary>
    /// Gets or sets the output helper for logging test output.
    /// </summary>
    public ITestOutputHelper OutputHelper { get; set; }

    /// <summary>
    /// Gets the settings for the integration tests.
    /// </summary>
    public readonly Settings Settings;

    private readonly RestClient _restClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommonHelper"/> class.
    /// </summary>
    public CommonHelper()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        Settings = new Settings();
        configuration.Bind(Settings);

        _restClient = new RestClient(Settings.WebApi.ApiBaseUrl);
    }

    /// <summary>
    /// Calls the specified API endpoint with the given parameters and request body.
    /// </summary>
    /// <param name="endPoint">The API endpoint to call.</param>
    /// <param name="endPointParams">The parameters to include in the endpoint URL.</param>
    /// <param name="method">The HTTP method to use for the request.</param>
    /// <param name="requestBody">The request body to send with the request.</param>
    /// <returns>The response from the API endpoint.</returns>
    public async Task<RestResponse> CallEndPoint(string endPoint, Dictionary<string, string>? endPointParams, Method method, object? requestBody)
    {
        if (endPointParams is { Count: > 0 })
        {
            endPoint = $"{endPoint}?";
            foreach (var (key, value) in endPointParams)
            {
                endPoint = $"{endPoint}{key}={value}&";
            }
            endPoint = endPoint.Remove(endPoint.Length - 1);
        }

        var request = new RestRequest(endPoint, method);

        if (requestBody != null)
        {
            request.AddJsonBody(requestBody);
        }

        var response = await _restClient.ExecuteAsync(request);

        if (response.Content != null)
        {
            OutputHelper.WriteLine(response.Content);
        }

        return response;
    }
}
