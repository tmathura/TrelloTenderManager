using TrelloTenderManager.Core.Interfaces;

namespace TrelloTenderManager.Core.Implementations;

public class TrelloTenderManager(ITenderCsvParser tenderCsvParser, ICardManager cardManager)
{
    public void PopulateCardsFromCsvFileContent(string fileContent)
    {
        var tenders = tenderCsvParser.Parse(fileContent);

        var tenderValidationResult = TenderValidator.Validate(tenders);

        foreach (var tender in tenderValidationResult.ValidTenders)
        {
            if (cardManager.Exists(tender))
            {
                cardManager.Update(tender);
            } else
            {
                cardManager.Create("", tender);
            }
        }
    }
}