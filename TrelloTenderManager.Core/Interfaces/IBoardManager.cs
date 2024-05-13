using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.Interfaces;

public interface IBoardManager
{
    Dictionary<TenderStatus, List> TenderStatusToListsOnBoardMapping { get; }
    HashSet<CustomField> CustomFieldsOnBoard { get; }
}