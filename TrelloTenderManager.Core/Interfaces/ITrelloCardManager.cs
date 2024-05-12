using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Interfaces;

public interface ITrelloCardManager
{
    void Create(Tender tender);
    void Update(Tender tender);
    bool Exists(Tender tender);
}