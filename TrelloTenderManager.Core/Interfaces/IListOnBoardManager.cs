using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.Interfaces;

public interface IListOnBoardManager
{
    Dictionary<TenderStatus, List> TenderStatusToListsOnBoardMapping { get; }
    void Setup();
}