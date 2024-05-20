using log4net;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Models.Queue;
using TrelloTenderManager.WebApi.Filters;

namespace TrelloTenderManager.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueueController(ICsvQueueBl cardManager) : ControllerBase
{
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    [HttpGet(Name = "GetQueue")]
    public async Task<List<CsvQueueDto>?> Get()
    {
        try
        {
            _logger.Info("Starting to get queue data");

            var csvQueueDaos = await cardManager.Read();

            var csvQueueDtos = csvQueueDaos.Adapt<List<CsvQueueDto>?>();

            _logger.Info("Got queue data successfully.");

            return csvQueueDtos;
        }
        catch (Exception exception)
        {
            throw new HttpResponseException((int)HttpStatusCode.InternalServerError, exception.Message);
        }
    }
}
