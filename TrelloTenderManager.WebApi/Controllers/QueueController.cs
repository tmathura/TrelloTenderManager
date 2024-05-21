using log4net;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Models.Queue;
using TrelloTenderManager.Domain.Requests;
using TrelloTenderManager.Domain.Responses;
using TrelloTenderManager.WebApi.Filters;

namespace TrelloTenderManager.WebApi.Controllers;

/// <summary>
/// Controller for managing the queue.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="QueueController"/> class.
/// </remarks>
/// <param name="cardManager">The CSV queue business logic.</param>
[ApiController]
[Route("api/[controller]")]
public class QueueController(ICsvQueueBl cardManager) : ControllerBase
{
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    /// <summary>
    /// Gets the queue data.
    /// </summary>
    /// <returns>The list of CSV queue DTOs.</returns>
    [HttpGet(Name = "GetQueue")]
    public async Task<GetQueueResponse> Get()
    {
        try
        {
            _logger.Info("Starting to get queue data");

            var csvQueueDaos = await cardManager.ReadCsvQueue();

            var csvQueueDtos = csvQueueDaos.Adapt<List<CsvQueueDto>?>();

            _logger.Info("Got queue data successfully.");

            return new GetQueueResponse(csvQueueDtos);
        }
        catch (Exception exception)
        {
            throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
        }
    }

    /// <summary>
    /// Queues the CSV data.
    /// </summary>
    /// <param name="queueFromCsvRequest">The request containing the CSV file content.</param>
    [HttpPost]
    [Route("queue-from-csv")]
    public async Task QueueFromCsv(QueueFromCsvRequest queueFromCsvRequest)
    {
        try
        {
            _logger.Info("Starting process to queue csv data.");

            await cardManager.QueueFromCsv(queueFromCsvRequest.Filename, queueFromCsvRequest.FileContent);

            _logger.Info("Csv data queued successfully.");
        }
        catch (Exception exception)
        {
            throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
        }
    }
}
