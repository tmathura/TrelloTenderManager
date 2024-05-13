using Moq;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Implementations;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Core.UnitTests.Helpers;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.UnitTests.Implementations;

public class ListOnBoardManagerTests
{
    private readonly Mock<ITrelloDotNetWrapper> _trelloDotNetWrapperMock;
    private readonly ListOnBoardManager _listOnBoardManager;

    public ListOnBoardManagerTests()
    {
        _trelloDotNetWrapperMock = new Mock<ITrelloDotNetWrapper>();
        _listOnBoardManager = new ListOnBoardManager(_trelloDotNetWrapperMock.Object, It.IsAny<string>());
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Setup_NoListsOnBoard_CreateListsOnBoard()
    {
        // Arrange
        var tenderStatuses = Enum.GetValues(typeof(TenderStatus)).Cast<TenderStatus>().ToList();

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetListsOnBoard(It.IsAny<string>())).ReturnsAsync((List<List>?)null);

        var listsOnBoard = new List<List>();

        foreach (var tenderStatus in tenderStatuses)
        {
            var listOnBoard = ListOnBoardHelper.GenerateBoardList(tenderStatus, true, true);
            listsOnBoard.Add(listOnBoard);

            _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.AddList(It.IsAny<string>(), tenderStatus.ToString())).ReturnsAsync(listOnBoard);
        }

        // Act
        _listOnBoardManager.Setup();

        // Assert
        foreach (var tenderStatus in tenderStatuses)
        {
            _trelloDotNetWrapperMock.Verify(x => x.AddList(It.IsAny<string>(), tenderStatus.ToString()), Times.Once);
        }

        Assert.Equal(tenderStatuses.Count, _listOnBoardManager.TenderStatusToListsOnBoardMapping.Count);

        foreach (var list in listsOnBoard)
        {
            var tenderStatus = Enum.Parse<TenderStatus>(list.Name);
            Assert.Contains(tenderStatus, _listOnBoardManager.TenderStatusToListsOnBoardMapping.Keys);
            Assert.Equal(list, _listOnBoardManager.TenderStatusToListsOnBoardMapping[tenderStatus]);
        }
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Setup_AllListsOnBoardExist_DoNotCreateLists()
    {
        // Arrange
        var tenderStatuses = Enum.GetValues(typeof(TenderStatus)).Cast<TenderStatus>().ToList();
        var listsOnBoard = ListOnBoardHelper.GenerateBoardLists(tenderStatuses, true, true);

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetListsOnBoard(It.IsAny<string>())).ReturnsAsync(listsOnBoard);

        // Act
        _listOnBoardManager.Setup();

        // Assert
        foreach (var tenderStatus in tenderStatuses)
        {
            _trelloDotNetWrapperMock.Verify(x => x.AddList(It.IsAny<string>(), tenderStatus.ToString()), Times.Never);
        }

        Assert.Equal(tenderStatuses.Count, _listOnBoardManager.TenderStatusToListsOnBoardMapping.Count);

        foreach (var list in listsOnBoard)
        {
            var tenderStatus = Enum.Parse<TenderStatus>(list.Name);
            Assert.Contains(tenderStatus, _listOnBoardManager.TenderStatusToListsOnBoardMapping.Keys);
            Assert.Equal(list, _listOnBoardManager.TenderStatusToListsOnBoardMapping[tenderStatus]);
        }
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Setup_ListsOnBoardExistWithMissingIds_ThrowException()
    {
        // Arrange
        var tenderStatuses = Enum.GetValues(typeof(TenderStatus)).Cast<TenderStatus>().ToList();
        var listsOnBoard = ListOnBoardHelper.GenerateBoardLists(tenderStatuses, false, true);

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetListsOnBoard(It.IsAny<string>())).ReturnsAsync(listsOnBoard);

        // Assert
        var exception = Assert.Throws<Exception>(Action);
        Assert.Equal($"Error getting list id for list with name: {tenderStatuses.Last()}", exception.Message);
        return;

        // Act
        void Action() => _listOnBoardManager.Setup();
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Setup_CreateListsOnBoardReturnsWithMissingIds_ThrowException()
    {
        // Arrange
        var tenderStatuses = Enum.GetValues(typeof(TenderStatus)).Cast<TenderStatus>().ToList();
        var listsOnBoard = new List<CustomField>();

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetCustomFieldsOnBoard(It.IsAny<string>())).ReturnsAsync(listsOnBoard);

        foreach (var tenderStatus in tenderStatuses)
        {
            var customField = ListOnBoardHelper.GenerateBoardList(tenderStatus, false, true);

            _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.AddList(It.IsAny<string>(), tenderStatus.ToString())).ReturnsAsync(customField);
        }

        // Assert
        var exception = Assert.Throws<Exception>(Action);
        Assert.Equal($"Error creating list on board with name: {tenderStatuses.Last()}", exception.Message);
        return;

        // Act
        void Action() => _listOnBoardManager.Setup();
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Setup_SomeCustomFieldsOnBoardExist_CreateMissingCustomFields()
    {
        // Arrange
        var tenderStatuses = Enum.GetValues(typeof(TenderStatus)).Cast<TenderStatus>().ToList();
        var firstHalfStatuses = tenderStatuses.Take(tenderStatuses.Count / 2).ToArray();
        var secondHalfStatuses = tenderStatuses.Skip(tenderStatuses.Count / 2).ToArray();
        var listsOnBoard = ListOnBoardHelper.GenerateBoardLists(firstHalfStatuses, true, true);

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetListsOnBoard(It.IsAny<string>())).ReturnsAsync(listsOnBoard);

        foreach (var tenderStatus in secondHalfStatuses)
        {
            var customField = ListOnBoardHelper.GenerateBoardList(tenderStatus, true, true);

            _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.AddList(It.IsAny<string>(), tenderStatus.ToString())).ReturnsAsync(customField);
        }

        // Act
        _listOnBoardManager.Setup();

        // Assert
        foreach (var tenderStatus in firstHalfStatuses)
        {
            _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.AddList(It.IsAny<string>(), tenderStatus.ToString()), Times.Never);
        }

        foreach (var tenderStatus in secondHalfStatuses)
        {
            _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.AddList(It.IsAny<string>(), tenderStatus.ToString()), Times.Once);
        }

        Assert.Equal(tenderStatuses.Count, _listOnBoardManager.TenderStatusToListsOnBoardMapping.Count);

        foreach (var list in listsOnBoard)
        {
            var tenderStatus = Enum.Parse<TenderStatus>(list.Name);
            Assert.Contains(tenderStatus, _listOnBoardManager.TenderStatusToListsOnBoardMapping.Keys);
            Assert.Equal(list, _listOnBoardManager.TenderStatusToListsOnBoardMapping[tenderStatus]);
        }
    }
}