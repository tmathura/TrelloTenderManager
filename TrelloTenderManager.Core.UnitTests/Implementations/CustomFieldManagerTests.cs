using Moq;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Implementations;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Core.UnitTests.Fakers;
using TrelloTenderManager.Core.UnitTests.Helpers;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.UnitTests.Implementations;

public class CustomFieldManagerTests
{
    private readonly TenderFaker _tenderFaker = new();

    private readonly Mock<ITrelloDotNetWrapper> _trelloDotNetWrapperMock;
    private readonly Mock<ICustomFieldOnBoardManager> _customFieldOnBoardManagerMock;
    private readonly CustomFieldManager _customFieldManager;

    public CustomFieldManagerTests()
    {
        _trelloDotNetWrapperMock = new Mock<ITrelloDotNetWrapper>();
        _customFieldOnBoardManagerMock = new Mock<ICustomFieldOnBoardManager>();

        _customFieldManager = new CustomFieldManager(_trelloDotNetWrapperMock.Object, _customFieldOnBoardManagerMock.Object);
    }

    [Fact, Trait("Category", "UnitTests")]
    public async Task UpdateCustomFieldsOnCard_OnlyUpdateValidCustomFields ()
    {
        // Arrange
        var card = new Card();
        var tender = _tenderFaker.Generate();
        var properties = typeof(Tender).GetProperties();
        var customFieldsOnBoard = CustomFieldOnBoardHelper.GenerateCustomFields(properties, true, true);

        _customFieldOnBoardManagerMock.Setup(m => m.CustomFieldsOnBoard).Returns(customFieldsOnBoard.ToHashSet);

        // Act
        await _customFieldManager.UpdateCustomFieldsOnCard(tender, card);

        // Assert
        foreach (var property in properties)
        {
            var customField = customFieldsOnBoard.First(customField => customField.Name == property.Name);
            var value = property.GetValue(tender);

            var valueAsString = CustomFieldManager.GetPropertyValueAsString(property, value);

            if (!string.IsNullOrWhiteSpace(valueAsString))
            {
                _trelloDotNetWrapperMock.Verify(m => m.UpdateCustomFieldValueOnCard(card.Id, customField, valueAsString), Times.Once);
            } 
            else
            {
                _trelloDotNetWrapperMock.Verify(m => m.UpdateCustomFieldValueOnCard(card.Id, customField, valueAsString), Times.Never);
            }
        }
    }

    [Fact, Trait("Category", "UnitTests")]
    public async Task UpdateCustomFieldsOnCardThrowException()
    {
        // Arrange
        var card = new Card();
        var tender = _tenderFaker.Generate();
        var properties = typeof(Tender).GetProperties();
        var customFieldsOnBoard = CustomFieldOnBoardHelper.GenerateCustomFields(properties, true, true);

        _customFieldOnBoardManagerMock.Setup(m => m.CustomFieldsOnBoard).Returns(customFieldsOnBoard.ToHashSet);
        _trelloDotNetWrapperMock.Setup(m => m.UpdateCustomFieldValueOnCard(It.IsAny<string>(), It.IsAny<CustomField>(), It.IsAny<string>())).Throws(new Exception());

        // Assert
        await Assert.ThrowsAsync<Exception>(Action);
        return;

        // Act
        async Task Action() => await _customFieldManager.UpdateCustomFieldsOnCard(tender, card);
    }
}