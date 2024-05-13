using System.Reflection;
using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.UnitTests.Helpers;

public static class ListOnBoardHelper
{

    public static List<List> GenerateBoardLists(IEnumerable<TenderStatus> tenderStatuses, bool includeId, bool includeName)
    {
        var listsOnBoard = new List<List>();

        foreach (var tenderStatus in tenderStatuses)
        {
            var customField = GenerateBoardList(tenderStatus, includeId, includeName);

            listsOnBoard.Add(customField);
        }

        return listsOnBoard;
    }

    public static List GenerateBoardList(TenderStatus tenderStatus, bool includeId, bool includeName)
    {
        var listOnBoard = new List();

        if (includeId)
        {
            var idPropertyInfo = listOnBoard.GetType().GetProperty(nameof(List.Id), BindingFlags.Public | BindingFlags.Instance);
            idPropertyInfo?.SetValue(listOnBoard, tenderStatus.ToString());
        }

        if (includeName)
        {
            var namePropertyInfo = listOnBoard.GetType().GetProperty(nameof(List.Name), BindingFlags.Public | BindingFlags.Instance);
            namePropertyInfo?.SetValue(listOnBoard, tenderStatus.ToString());
        }

        return listOnBoard;
    }
}