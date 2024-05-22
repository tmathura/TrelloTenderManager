using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.Managers.Interfaces;

public interface IBoardManager
{
    /// <summary>
    /// Gets the ID of the board.
    /// </summary>
    string BoardId { get; }

    /// <summary>
    /// Gets the mapping of tender status to lists on the board.
    /// </summary>
    Dictionary<TenderStatus, List> TenderStatusToListsOnBoardMapping { get; }

    /// <summary>
    /// Gets the custom fields on the board.
    /// </summary>
    HashSet<CustomField> CustomFieldsOnBoard { get; }
}