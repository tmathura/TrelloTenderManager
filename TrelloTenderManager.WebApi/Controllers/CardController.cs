using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Requests;
using TrelloTenderManager.Domain.Responses;
using TrelloTenderManager.WebApi.Filters;

namespace TrelloTenderManager.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardController(ICardManager cardManager) : ControllerBase
{
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
    
    /// <summary>
    /// Processes the CSV data.
    /// </summary>
    /// <param name="processFromCsvRequest">The request containing the CSV file content.</param>
    /// <returns>The response containing the processed data.</returns>
    [HttpPost]
    [Route("process-from-csv")]
    public async Task<ProcessFromCsvResponse> ProcessFromCsv(ProcessFromCsvRequest processFromCsvRequest)
    {
        try
        {
            _logger.Info($"Starting to process csv data: {JsonSerializer.Serialize(processFromCsvRequest.FileContent)}.");

            var processFromCsv = await cardManager.ProcessFromCsv(processFromCsvRequest.FileContent);

            _logger.Info($"Csv data processed with result: {JsonSerializer.Serialize(processFromCsv)}.");

            return new ProcessFromCsvResponse(processFromCsv);
        }
        catch (Exception exception)
        {
            throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
        }
    }
}
