using Microsoft.Extensions.Configuration;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Managers.Interfaces;
using TrelloTenderManager.Core.Wrappers.Interfaces;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Domain.Exceptions;

namespace TrelloTenderManager.Core.Managers.Implementations;

/// <summary>
/// Represents a manager for lists on a Trello board.
/// </summary>
public class ListOnBoardManager : IListOnBoardManager
{
    /// <summary>
    /// The TrelloDotNetWrapper instance.
    /// </summary>
    private readonly ITrelloDotNetWrapper _trelloDotNetWrapper;

    /// <summary>
    /// The ID of the Trello board.
    /// </summary>
    private static string _boardId = string.Empty;

    /// <inheritdoc />
    public Dictionary<TenderStatus, List> TenderStatusToListsOnBoardMapping { get; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ListOnBoardManager"/> class.
    /// </summary>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="trelloDotNetWrapper">The TrelloDotNetWrapper instance.</param>
    /// <exception cref="AppSettingsException">Thrown when an error occurs getting the board ID from the configuration.</exception>
    public ListOnBoardManager(IConfiguration configuration, ITrelloDotNetWrapper trelloDotNetWrapper)
    {
        _trelloDotNetWrapper = trelloDotNetWrapper;
        var boardId = configuration["Trello:BoardId"];

        if (string.IsNullOrWhiteSpace(boardId))
        {
            throw new AppSettingsException("Error getting board ID from configuration.");
        }

        _boardId = boardId;
    }

    /// <inheritdoc />
    public void Setup()
    {
        var listsOnBoard = _trelloDotNetWrapper.GetListsOnBoard(_boardId).Result;
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
            var listOnBoard = _trelloDotNetWrapper.AddList(_boardId, tenderStatus.ToString()).Result;

            if (string.IsNullOrWhiteSpace(listOnBoard?.Id))
            {
                throw new Exception($"Error creating list on board with name: {tenderStatus}");
            }

            TenderStatusToListsOnBoardMapping.Add(tenderStatus, listOnBoard);
        }
    }
}
