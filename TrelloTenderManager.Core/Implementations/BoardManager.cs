using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Domain.Exceptions;

namespace TrelloTenderManager.Core.Implementations;

/// <summary>
/// Represents a manager for Trello boards.
/// </summary>
public class BoardManager : IBoardManager
{
    private readonly IListOnBoardManager _listOnBoardManager;
    private readonly ICustomFieldOnBoardManager _customFieldOnBoardManager;

    /// <inheritdoc />
    public string BoardId { get; }

    /// <inheritdoc />
    public Dictionary<TenderStatus, List> TenderStatusToListsOnBoardMapping => _listOnBoardManager.TenderStatusToListsOnBoardMapping;

    /// <inheritdoc />
    public HashSet<CustomField> CustomFieldsOnBoard => _customFieldOnBoardManager.CustomFieldsOnBoard;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoardManager"/> class.
    /// </summary>
    /// <param name="listOnBoardManager">The list on board manager.</param>
    /// <param name="customFieldOnBoardManager">The custom field on board manager.</param>
    /// <param name="boardId">The ID of the board.</param>
    public BoardManager(IListOnBoardManager listOnBoardManager, ICustomFieldOnBoardManager customFieldOnBoardManager, string boardId)
    {
        _listOnBoardManager = listOnBoardManager;
        _customFieldOnBoardManager = customFieldOnBoardManager;

        BoardId = boardId;

        SetUp(boardId);
    }

    /// <summary>
    /// Sets up the board.
    /// </summary>
    /// <param name="boardId">The ID of the board.</param>
    private void SetUp(string boardId)
    {
        try
        {
            _listOnBoardManager.Setup();
            _customFieldOnBoardManager.Setup();
        }
        catch (Exception exception)
        {
            throw new BoardSetupException($"Error setting up board: {boardId}", exception);
        }
    }
}
