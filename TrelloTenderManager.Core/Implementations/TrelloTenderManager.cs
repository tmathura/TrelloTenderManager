using TrelloTenderManager.Core.Interfaces;

namespace TrelloTenderManager.Core.Implementations;

public class TrelloTenderManager(ITenderCsvParser tenderCsvParser, ITrelloCardManager trelloCardManager)
{
    public void PopulateCardsFromCsvFileContent(string fileContent)
    {
        var tenders = tenderCsvParser.Parse(fileContent);

        var tenderValidationResult = TenderValidator.Validate(tenders);

        foreach (var tender in tenderValidationResult.ValidTenders)
        {
            if (trelloCardManager.Exists(tender))
            {
                trelloCardManager.Update(tender);
            } else
            {
                trelloCardManager.Create(tender);
            }
        }
    }
}