using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.CsvClassMaps;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Implementations;

public class TenderCsvParser(ICsvHelperWrapper csvHelperWrapper) : ITenderCsvParser
{
    public List<Tender> Parse(string fileContent)
    {
        var tenders = csvHelperWrapper.GetRecords<Tender>(fileContent, typeof(TenderMap));

        return tenders;
    }
}