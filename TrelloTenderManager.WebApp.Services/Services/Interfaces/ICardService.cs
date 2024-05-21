using TrelloTenderManager.Domain.Requests;
using TrelloTenderManager.Domain.Responses;

namespace TrelloTenderManager.WebApp.Services.Services.Interfaces;

/// <summary>
/// Represents a service for managing Trello cards.
/// </summary>
public interface ICardService
{
    /// <summary>
    /// Processes a CSV file and creates Trello cards based on the data.
    /// </summary>
    /// <param name="processFromCsvRequest">The request containing the CSV file data.</param>
    /// <returns>The response containing the result of the CSV processing.</returns>
    Task<ProcessFromCsvResponse> ProcessCsv(ProcessFromCsvRequest processFromCsvRequest);
}
