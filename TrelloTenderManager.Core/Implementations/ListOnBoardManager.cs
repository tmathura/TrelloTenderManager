using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.Implementations;

public class ListOnBoardManager(ITrelloDotNetWrapper trelloDotNetWrapper, string boardId) : IListOnBoardManager
{

    public Dictionary<TenderStatus, List> TenderStatusToListsOnBoardMapping { get; } = [];

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