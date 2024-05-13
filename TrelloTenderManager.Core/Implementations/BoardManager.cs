using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Domain.Exceptions;

namespace TrelloTenderManager.Core.Implementations;

public class BoardManager : IBoardManager
{
    private readonly IListOnBoardManager _listOnBoardManager;
    private readonly ICustomFieldOnBoardManager _customFieldOnBoardManager;
    public string BoardId { get; }
    public Dictionary<TenderStatus, List> TenderStatusToListsOnBoardMapping => _listOnBoardManager.TenderStatusToListsOnBoardMapping;
    public HashSet<CustomField> CustomFieldsOnBoard => _customFieldOnBoardManager.CustomFieldsOnBoard;

    public BoardManager(IListOnBoardManager listOnBoardManager, ICustomFieldOnBoardManager customFieldOnBoardManager, string boardId)
    {
        _listOnBoardManager = listOnBoardManager;
        _customFieldOnBoardManager = customFieldOnBoardManager;

        BoardId = boardId;

        SetUp(boardId);
    }

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