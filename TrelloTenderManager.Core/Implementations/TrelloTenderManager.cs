using TrelloTenderManager.Core.Interfaces;

namespace TrelloTenderManager.Core.Implementations;

/// <summary>
/// Represents a manager for handling Trello tenders.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TrelloTenderManager"/> class.
/// </remarks>
/// <param name="tenderCsvParser">The tender CSV parser.</param>
/// <param name="cardManager">The card manager.</param>
public class TrelloTenderManager(ITenderCsvParser tenderCsvParser, ICardManager cardManager) : ITrelloTenderManager
{
    /// <inheritdoc />
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
            }
            else
            {
                await cardManager.Create(tender);
            }
        }
    }
}
