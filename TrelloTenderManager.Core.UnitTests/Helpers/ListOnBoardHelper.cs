using System.Reflection;
using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.UnitTests.Helpers;

/// <summary>
/// Helper class for generating board lists.
/// </summary>
public static class ListOnBoardHelper
{
    /// <summary>
    /// Generates board lists based on the provided tender statuses.
    /// </summary>
    /// <param name="tenderStatuses">The tender statuses.</param>
    /// <param name="includeId">Indicates whether to include the ID property in the generated lists.</param>
    /// <param name="includeName">Indicates whether to include the Name property in the generated lists.</param>
    /// <returns>The generated board lists.</returns>
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

    /// <summary>
    /// Generates a board list based on the provided tender status.
    /// </summary>
    /// <param name="tenderStatus">The tender status.</param>
    /// <param name="includeId">Indicates whether to include the ID property in the generated list.</param>
    /// <param name="includeName">Indicates whether to include the Name property in the generated list.</param>
    /// <returns>The generated board list.</returns>
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
