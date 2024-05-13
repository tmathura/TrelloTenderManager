using TrelloTenderManager.Core.Interfaces;

namespace TrelloTenderManager.Core.Implementations;

public class TrelloTenderManager(ITenderCsvParser tenderCsvParser, ICardManager cardManager)
{
    public async Task PopulateCardsFromCsvFileContent(string fileContent)
    {
        var tenders = tenderCsvParser.Parse(fileContent);

        var tenderValidationResult = TenderValidator.Validate(tenders);

        foreach (var tender in tenderValidationResult.ValidTenders)
        {
            var card = await cardManager.Exists(tender);
            if (card is not null)
            {
                await cardManager.Update(card, tender);
            } else
            {
                await cardManager.Create(tender);
            }
        }
    }
}