using Microsoft.Extensions.Configuration;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Managers.Interfaces;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Domain.Exceptions;

namespace TrelloTenderManager.Core.Managers.Implementations;

/// <summary>
/// Represents a manager for Trello boards.
/// </summary>
public class BoardManager : IBoardManager
{
    /// <summary>
    /// The list on board manager.
    /// </summary>
    private readonly IListOnBoardManager _listOnBoardManager;

    /// <summary>
    /// The custom field on board manager.
    /// </summary>
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
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="listOnBoardManager">The list on board manager.</param>
    /// <param name="customFieldOnBoardManager">The custom field on board manager.</param>
    public BoardManager(IConfiguration configuration, IListOnBoardManager listOnBoardManager, ICustomFieldOnBoardManager customFieldOnBoardManager)
    {
        _listOnBoardManager = listOnBoardManager;
        _customFieldOnBoardManager = customFieldOnBoardManager;

        var boardId = configuration["Trello:BoardId"];

        if (string.IsNullOrWhiteSpace(boardId))
        {
            throw new AppSettingsException("Error getting board ID from configuration.");
        }

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
