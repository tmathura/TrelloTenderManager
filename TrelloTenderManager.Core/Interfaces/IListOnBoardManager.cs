using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.Interfaces;

public interface IListOnBoardManager
{
    /// <summary>
    /// Gets the mapping of tender status to lists on the board.
    /// </summary>
    Dictionary<TenderStatus, List> TenderStatusToListsOnBoardMapping { get; }

    /// <summary>
    /// Sets up the lists on the board based on the tender status.
    /// </summary>
    void Setup();
}