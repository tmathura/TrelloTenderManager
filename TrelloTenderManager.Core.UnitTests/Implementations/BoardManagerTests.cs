using Moq;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Implementations;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Core.UnitTests.Helpers;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Domain.Exceptions;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.UnitTests.Implementations;

public class BoardManagerTests
{
    private readonly Mock<IListOnBoardManager> _listOnBoardManagerMock;
    private readonly Mock<ICustomFieldOnBoardManager> _customFieldOnBoardManagerMock;
    private readonly BoardManager _boardManager;

    public BoardManagerTests()
    {
        _listOnBoardManagerMock = new Mock<IListOnBoardManager>();
        _customFieldOnBoardManagerMock = new Mock<ICustomFieldOnBoardManager>();

        _boardManager = new BoardManager(_listOnBoardManagerMock.Object, _customFieldOnBoardManagerMock.Object, It.IsAny<string>());
    }

    [Fact, Trait("Category", "UnitTests")]
    public void TenderStatusToListsOnBoardMapping_Should_Return_ListsMapping()
    {
        // Arrange
        var tenderStatusToListsOnBoardMapping = new Dictionary<TenderStatus, List>();
        var tenderStatuses = Enum.GetValues(typeof(TenderStatus)).Cast<TenderStatus>().ToList();

        foreach (var tenderStatus in tenderStatuses)
        {
            var list = new List { Name = tenderStatus.ToString() };
            tenderStatusToListsOnBoardMapping.Add(tenderStatus, list);
        }

        _listOnBoardManagerMock.Setup(listOnBoardManager => listOnBoardManager.TenderStatusToListsOnBoardMapping).Returns(tenderStatusToListsOnBoardMapping);

        // Act
        var actualListsOnBoard = _boardManager.TenderStatusToListsOnBoardMapping;

        // Assert
        Assert.Equal(tenderStatusToListsOnBoardMapping, actualListsOnBoard);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void CustomFieldsOnBoard_Should_Return_CustomFields()
    {
        // Arrange
        var properties = typeof(Tender).GetProperties();
        var customFieldsOnBoard = CustomFieldOnBoardHelper.GenerateCustomFields(properties, true, true);

        _customFieldOnBoardManagerMock.Setup(customFieldOnBoardManager => customFieldOnBoardManager.CustomFieldsOnBoard).Returns(customFieldsOnBoard.ToHashSet);

        // Act
        var actualCustomFieldsOnBoard = _boardManager.CustomFieldsOnBoard;

        // Assert
        Assert.Equal(customFieldsOnBoard.OrderBy(customField => customField.Id), actualCustomFieldsOnBoard.OrderBy(customField => customField.Id));
    }

    [Fact, Trait("Category", "UnitTests")]
    public void SetUp_Should_Call_Setup_Methods()
    {
        // Assert
        _listOnBoardManagerMock.Verify(m => m.Setup(), Times.Once);
        _customFieldOnBoardManagerMock.Verify(m => m.Setup(), Times.Once);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void SetUp_Should_Throw_BoardSetupException_When_Setup_Fails()
    {
        // Arrange
        const string boardId = "MyBoard1989";
        _listOnBoardManagerMock.Setup(m => m.Setup()).Throws(new Exception());
        
        // Assert
        var exception = Assert.Throws<BoardSetupException>(Action);
        Assert.Equal($"Error setting up board: {boardId}", exception.Message);
        return;

        // Act
        void Action() => _ = new BoardManager(_listOnBoardManagerMock.Object, _customFieldOnBoardManagerMock.Object, boardId);
    }
}