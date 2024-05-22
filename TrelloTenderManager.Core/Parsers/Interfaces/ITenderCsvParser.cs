using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Parsers.Interfaces;

public interface ITenderCsvParser
{
    /// <summary>
    /// Parses the specified CSV file content into a list of tenders.
    /// </summary>
    /// <param name="fileContent">The content of the CSV file.</param>
    /// <returns>A list of tenders.</returns>
    List<Tender> Parse(string? fileContent);
}