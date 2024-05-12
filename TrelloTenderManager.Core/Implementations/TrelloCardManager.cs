using TrelloTenderManager.Core.Interfaces;

namespace TrelloTenderManager.Core.Implementations;

public class TrelloCardManager(ITenderCsvParser tenderCsvParser)
{
    public void PopulateFromCsvFileContent(string fileContent)
    {
        var tenders = tenderCsvParser.Parse(fileContent);

        var tenderValidationResult = TenderValidator.Validate(tenders);

        foreach (var tender in tenderValidationResult.ValidTenders)
        {
            // Create Trello card
        }
    }
}