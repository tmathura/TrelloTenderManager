using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Implementations;

public class CardManager(ITrelloDotNetWrapper trelloDotNetWrapper) : ICardManager
{
    public async Task Create(string cardListId, Tender tender)
    {
        var card = new Card(cardListId, tender.Name, tender.TenderName);

        var newCard = await trelloDotNetWrapper.AddCard(card);
    }

    public void Update(Tender tender)
    {
        throw new NotImplementedException();
    }

    public bool Exists(Tender tender)
    {
        throw new NotImplementedException();
    }
}