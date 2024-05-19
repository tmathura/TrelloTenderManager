using Moq;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Implementations;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Core.UnitTests.Fakers;
using TrelloTenderManager.Core.UnitTests.Helpers;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.UnitTests.Implementations;

public class CardManagerTests
{
    private readonly TenderFaker _tenderFaker = new();

    private readonly Mock<ITrelloDotNetWrapper> _trelloDotNetWrapperMock;
    private readonly Mock<IBoardManager> _boardManagerMock;
    private readonly Mock<ICustomFieldManager> _customFieldManagerMock;
    private readonly CardManager _cardManager;

    public CardManagerTests()
    {
        var csvQueueBlMock = new Mock<ICsvQueueBl>();
        var tenderCsvParserMock = new Mock<ITenderCsvParser>();
        _trelloDotNetWrapperMock = new Mock<ITrelloDotNetWrapper>();
        _boardManagerMock = new Mock<IBoardManager>();
        _customFieldManagerMock = new Mock<ICustomFieldManager>();

        _cardManager = new CardManager(csvQueueBlMock.Object, tenderCsvParserMock.Object, _trelloDotNetWrapperMock.Object, _boardManagerMock.Object, _customFieldManagerMock.Object);
    }

    [Fact, Trait("Category", "UnitTests")]
    public async Task Create_Valid()
    {
        // Arrange
        var tender = _tenderFaker.Generate();
        var listOnBoard = ListOnBoardHelper.GenerateBoardList(tender.Status, true, true);
        var card = new Card(listOnBoard.Id, tender.Name, CardManager.GetCardDescription(tender));
        var expectedCard = new Card();

        _boardManagerMock.Setup(boardManager => boardManager.TenderStatusToListsOnBoardMapping).Returns(new Dictionary<TenderStatus, List>
        {
            { tender.Status, listOnBoard }
        });

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.AddCard(It.Is<Card>(data => data.Id == card.Id && data.Name == card.Name && data.Description == card.Description))).ReturnsAsync(expectedCard);

        // Act
        var actualCard = await _cardManager.Create(tender);

        // Assert
        Assert.Equal(expectedCard, actualCard);
        _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.AddCard(It.Is<Card>(data => data.Id == card.Id && data.Name == card.Name && data.Description == card.Description)), Times.Once);
        _customFieldManagerMock.Verify(customFieldManager => customFieldManager.UpdateCustomFieldsOnCard(tender, expectedCard), Times.Once);
    }

    [Fact, Trait("Category", "UnitTests")]
    public async Task Update_Valid()
    {
        // Arrange
        var tender = _tenderFaker.Generate();
        var listOnBoard = ListOnBoardHelper.GenerateBoardList(tender.Status, true, true);
        var card = new Card(listOnBoard.Id, tender.Name, CardManager.GetCardDescription(tender));
        var expectedCard = new Card();

        _boardManagerMock.Setup(boardManager => boardManager.TenderStatusToListsOnBoardMapping).Returns(new Dictionary<TenderStatus, List>
        {
            { tender.Status, listOnBoard }
        });

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.UpdateCard(It.Is<Card>(data => data.Id == card.Id && data.Name == card.Name && data.Description == card.Description))).ReturnsAsync(expectedCard);

        // Act
        await _cardManager.Update(card, tender);

        // Assert
        _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.UpdateCard(It.Is<Card>(data => data.Id == card.Id && data.Name == card.Name && data.Description == card.Description)), Times.Once);
        _customFieldManagerMock.Verify(customFieldManager => customFieldManager.UpdateCustomFieldsOnCard(tender, expectedCard), Times.Once);
    }

    [Fact, Trait("Category", "UnitTests")]
    public async Task Exists_Found()
    {
        // Arrange
        const string cardId = "MyCardId19";

        var tender = _tenderFaker.Generate();
        var card = CardHelper.GenerateCard(tender, cardId);

        var searchViaBoardResult = new SearchViaBoardResult{ Id = cardId, Name = tender.Name, Desc = CardManager.GetCardDescription(tender) };

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.SearchOnCardViaBoard(It.IsAny<string>(), CardManager.GetCardDescription(tender))).ReturnsAsync(searchViaBoardResult);
        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetCard(cardId)).ReturnsAsync(card);

        // Act
        var result = await _cardManager.Exists(tender);

        // Assert
        Assert.NotNull(result);
        _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.SearchOnCardViaBoard(It.IsAny<string>(), CardManager.GetCardDescription(tender)), Times.Once);
        _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.GetCard(cardId), Times.Once);
    }

    [Fact, Trait("Category", "UnitTests")]
    public async Task Exists_NotFound()
    {
        // Arrange
        var tender = _tenderFaker.Generate();

        // Act
        var result = await _cardManager.Exists(tender);

        // Assert
        Assert.Null(result);
        _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.SearchOnCardViaBoard(It.IsAny<string>(), CardManager.GetCardDescription(tender)), Times.Once);
        _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.GetCard(It.IsAny<string>()), Times.Never);
    }
}