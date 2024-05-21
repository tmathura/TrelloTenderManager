using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.CsvClassMaps;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Implementations;

/// <summary>
/// Represents a CSV parser for tender data.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TenderCsvParser"/> class.
/// </remarks>
/// <param name="csvHelperWrapper">The CSV helper wrapper.</param>
public class TenderCsvParser(ICsvHelperWrapper csvHelperWrapper) : ITenderCsvParser
{
    /// <inheritdoc />
    public List<Tender> Parse(string? fileContent)
    {
        var tenders = csvHelperWrapper.GetRecords<Tender>(fileContent, typeof(TenderMap));

        return tenders;
    }
}
