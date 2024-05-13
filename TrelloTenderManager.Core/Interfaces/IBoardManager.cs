using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.Interfaces;

public interface IBoardManager
{
    string BoardId { get; }
    Dictionary<TenderStatus, List> TenderStatusToListsOnBoardMapping { get; }
    HashSet<CustomField> CustomFieldsOnBoard { get; }
}