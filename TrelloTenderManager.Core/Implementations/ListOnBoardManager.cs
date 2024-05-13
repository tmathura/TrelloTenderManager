using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.Implementations;

/// <summary>
/// Manages the lists on a Trello board based on the tender status.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ListOnBoardManager"/> class.
/// </remarks>
/// <param name="trelloDotNetWrapper">The TrelloDotNetWrapper instance.</param>
/// <param name="boardId">The ID of the Trello board.</param>
public class ListOnBoardManager(ITrelloDotNetWrapper trelloDotNetWrapper, string boardId) : IListOnBoardManager
{
    /// <inheritdoc />
    public Dictionary<TenderStatus, List> TenderStatusToListsOnBoardMapping { get; } = [];

    /// <inheritdoc />
    public void Setup()
    {
        var listsOnBoard = trelloDotNetWrapper.GetListsOnBoard(boardId).Result;
        var tenderStatuses = Enum.GetValues(typeof(TenderStatus)).Cast<TenderStatus>().Reverse().ToList();

        if (listsOnBoard is null || listsOnBoard.Count == 0)
        {
            CreateListsOnBoard(tenderStatuses);
        }
        else
        {
            var tenderStatusesToCreateFrom = tenderStatuses.ToHashSet();

            foreach (var tenderStatus in tenderStatuses)
            {
                var listOnBoard = listsOnBoard.FirstOrDefault(list => list.Name == tenderStatus.ToString());

                if (listOnBoard is not null)
                {
                    if (string.IsNullOrWhiteSpace(listOnBoard.Id))
                    {
                        throw new Exception($"Error getting list id for list with name: {tenderStatus}");
                    }

                    TenderStatusToListsOnBoardMapping.Add(tenderStatus, listOnBoard);
                    tenderStatusesToCreateFrom.Remove(tenderStatus);
                }
            }

            CreateListsOnBoard(tenderStatusesToCreateFrom);
        }
    }

    /// <summary>
    /// Creates the lists on the board for the specified tender statuses.
    /// </summary>
    /// <param name="tenderStatuses">The tender statuses to create lists for.</param>
    private void CreateListsOnBoard(IEnumerable<TenderStatus> tenderStatuses)
    {
        foreach (var tenderStatus in tenderStatuses)
        {
            var listOnBoard = trelloDotNetWrapper.AddList(boardId, tenderStatus.ToString()).Result;

            if (string.IsNullOrWhiteSpace(listOnBoard?.Id))
            {
                throw new Exception($"Error creating list on board with name: {tenderStatus}");
            }

            TenderStatusToListsOnBoardMapping.Add(tenderStatus, listOnBoard);
        }
    }
}
