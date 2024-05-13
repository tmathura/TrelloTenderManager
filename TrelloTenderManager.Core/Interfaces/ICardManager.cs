using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Interfaces;

public interface ICardManager
{
    Task Create(string cardListId, Tender tender);
    void Update(Tender tender);
    bool Exists(Tender tender);
}