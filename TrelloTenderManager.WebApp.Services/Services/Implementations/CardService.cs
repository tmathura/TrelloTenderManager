using log4net;
using RestSharp;
using System.Net;
using TrelloTenderManager.Domain.Requests;
using TrelloTenderManager.Domain.Responses;
using TrelloTenderManager.WebApp.Services.Services.Interfaces;

namespace TrelloTenderManager.WebApp.Services.Services.Implementations;

/// <summary>
/// Represents a service for managing cards.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CardService"/> class.
/// </remarks>
/// <param name="restSharpClient">The REST client.</param>
public class CardService(IRestClient restSharpClient) : ICardService
{
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    /// <inheritdoc />
    public async Task<ProcessFromCsvResponse> ProcessCsv(ProcessFromCsvRequest processFromCsvRequest)
    {
        const string apiEndpoint = "api/card/process-from-csv";

        var request = new RestRequest(apiEndpoint);
        request.AddJsonBody(processFromCsvRequest);

        var response = await restSharpClient.ExecutePostAsync<ProcessFromCsvResponse>(request);

        if (response.StatusCode == HttpStatusCode.OK) return response.Data;

        var errorResponseMessage = response.Content;

        if (response.StatusCode == 0)
        {
            errorResponseMessage = "There is an issue with the API.";
        }

        var errorResponse = $"HTTP status code is: {response.StatusCode}. {errorResponseMessage}.";
        _logger.Error($"Error calling API endpoint {apiEndpoint}. {errorResponse}");

        throw new Exception(errorResponseMessage);
    }
}
