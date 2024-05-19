using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Domain.Responses;

/// <summary>
/// Represents the response of processing a CSV file.
/// </summary>
/// <param name="processFromCsvResult">The result of processing the CSV file.</param>
public class ProcessFromCsvResponse(ProcessFromCsvResult processFromCsvResult)
{
    /// <summary>
    /// Gets or sets the result of processing the CSV file.
    /// </summary>
    public ProcessFromCsvResult ProcessFromCsvResult { get; set; } = processFromCsvResult;
}
