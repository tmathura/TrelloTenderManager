using log4net;
using RestSharp;
using System.Net;
using TrelloTenderManager.Domain.Models.Queue;
using TrelloTenderManager.Domain.Requests;
using TrelloTenderManager.Domain.Responses;
using TrelloTenderManager.WebApp.Services.Services.Interfaces;

namespace TrelloTenderManager.WebApp.Services.Services.Implementations;

/// <summary>
/// Service implementation for queue operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="QueueService"/> class.
/// </remarks>
/// <param name="restSharpClient">The REST client.</param>
public class QueueService(IRestClient restSharpClient) : IQueueService
{
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    /// <inheritdoc />
    public async Task QueueCsv(QueueFromCsvRequest queueFromCsvRequest)
    {
        const string apiEndpoint = "api/queue/queue-from-csv";

        var request = new RestRequest(apiEndpoint);
        request.AddJsonBody(queueFromCsvRequest);

        var response = await restSharpClient.ExecutePostAsync(request);

        if (response.StatusCode == HttpStatusCode.OK) return;

        var errorResponse = $"Http status code is: {response.StatusCode}. {response.Content}.";
        _logger.Error($"Error calling api endpoint {apiEndpoint}. {errorResponse}");

        throw new Exception(response.Content);
    }

    /// <inheritdoc />
    public async Task<List<CsvQueueDto>?> GetQueue()
    {
        const string apiEndpoint = "api/queue";

        var request = new RestRequest(apiEndpoint);

        var response = await restSharpClient.ExecuteGetAsync<GetQueueResponse>(request);

        if (response.StatusCode == HttpStatusCode.OK) return response.Data?.CsvQueueItems;

        var errorResponseMessage = response.Content;

        if (response.StatusCode == 0)
        {
            errorResponseMessage = "There is an issue with the api.";
        }

        var errorResponse = $"Http status code is: {response.StatusCode}. {errorResponseMessage}.";
        _logger.Error($"Error calling api endpoint {apiEndpoint}. {errorResponse}");

        throw new Exception(errorResponseMessage);
    }
}
