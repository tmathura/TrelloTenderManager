using System.Security.Cryptography;
using System.Text;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Implementations;

public class CardManager(ITrelloDotNetWrapper trelloDotNetWrapper, IBoardManager boardManager, ICustomFieldManager customFieldManager) : ICardManager
{
    public async Task<Card> Create(Tender tender)
    {
        var cardListId = boardManager.TenderStatusToListsOnBoardMapping[tender.Status].Id;
        
        var card = new Card(cardListId, tender.Name, GetCardDescription(tender));

        var cardResult = await trelloDotNetWrapper.AddCard(card);

        await customFieldManager.UpdateCustomFieldsOnCard(tender, cardResult);

        return cardResult;
    }

    public async Task Update(Card card, Tender tender)
    {
        card.Name = tender.Name;
        card.Description = GetCardDescription(tender);

        var cardResult = await trelloDotNetWrapper.UpdateCard(card);

        await customFieldManager.UpdateCustomFieldsOnCard(tender, cardResult);
    }

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