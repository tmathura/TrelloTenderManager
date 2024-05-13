using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Interfaces;

public interface ICardManager
{
    Task<Card> Create(Tender tender);
    Task Update(Card card, Tender tender);
    Task<Card?> Exists(Tender tender);
}