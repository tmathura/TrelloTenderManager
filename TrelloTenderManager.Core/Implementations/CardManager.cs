using System.Security.Cryptography;
using System.Text;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Implementations;

/// <summary>
/// Manages the creation, update, and existence of Trello cards for tenders.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CardManager"/> class.
/// </remarks>
/// <param name="trelloDotNetWrapper">The TrelloDotNetWrapper instance.</param>
/// <param name="boardManager">The BoardManager instance.</param>
/// <param name="customFieldManager">The CustomFieldManager instance.</param>
public class CardManager(ITrelloDotNetWrapper trelloDotNetWrapper, IBoardManager boardManager, ICustomFieldManager customFieldManager) : ICardManager
{
    /// <inheritdoc />
    public async Task<Card> Create(Tender tender)
    {
        var cardListId = boardManager.TenderStatusToListsOnBoardMapping[tender.Status].Id;

        var card = new Card(cardListId, tender.Name, GetCardDescription(tender));

        var cardResult = await trelloDotNetWrapper.AddCard(card);

        await customFieldManager.UpdateCustomFieldsOnCard(tender, cardResult);

        return cardResult;
    }

    /// <inheritdoc />
    public async Task Update(Card card, Tender tender)
    {
        card.Name = tender.Name;
        card.Description = GetCardDescription(tender);

        var cardResult = await trelloDotNetWrapper.UpdateCard(card);

        await customFieldManager.UpdateCustomFieldsOnCard(tender, cardResult);
    }

    /// <inheritdoc />
    public async Task<Card?> Exists(Tender tender)
    {
        var card = (Card?)null;
        var cardDescription = GetCardDescription(tender);

        if (!string.IsNullOrWhiteSpace(cardDescription))
        {
            card = await trelloDotNetWrapper.SearchOnCard(boardManager.BoardId, cardDescription);
        }

        return card;
    }

    /// <summary>
    /// Gets the description for a Trello card based on the specified tender.
    /// </summary>
    /// <param name="tender">The tender to generate the description for.</param>
    /// <returns>The generated card description.</returns>
    public static string GetCardDescription(Tender tender)
    {
        var uniqueId = GetUniqueId(tender);

        return $"Unique Id - {uniqueId}";
    }

    private static string GetUniqueId(Tender tender)
    {
        var tenderIdentification = tender.Id + tender.LotNumber;
        var data = MD5.HashData(Encoding.UTF8.GetBytes(tenderIdentification));

        return BitConverter.ToString(data).Replace("-", "").ToLower();
    }
}
