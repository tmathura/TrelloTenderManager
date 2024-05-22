using Microsoft.Extensions.Configuration;
using Moq;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Managers.Implementations;
using TrelloTenderManager.Core.UnitTests.Helpers;
using TrelloTenderManager.Core.Wrappers.Interfaces;
using TrelloTenderManager.Domain.Exceptions;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.UnitTests.Implementations;

public class CustomFieldOnBoardManagerTests
{
    private const string BoardId = "MyBoard1989";

    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ITrelloDotNetWrapper> _trelloDotNetWrapperMock;
    private readonly CustomFieldOnBoardManager _customFieldOnBoardManager;

    public CustomFieldOnBoardManagerTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["Trello:BoardId"]).Returns(BoardId);

        _trelloDotNetWrapperMock = new Mock<ITrelloDotNetWrapper>();

        _customFieldOnBoardManager = new CustomFieldOnBoardManager(_configurationMock.Object, _trelloDotNetWrapperMock.Object);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Setup_NoCustomFieldsOnBoard_CreateCustomFields()
    {
        // Arrange
        var properties = typeof(Tender).GetProperties();

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetCustomFieldsOnBoard(It.IsAny<string>())).ReturnsAsync((List<CustomField>?)null);

        var customFieldsOnBoard = new List<CustomField>();

        foreach (var property in properties)
        {
            var customField = CustomFieldOnBoardHelper.GenerateCustomField(property, true, true);
            customFieldsOnBoard.Add(customField);

            _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.AddCustomFieldToBoard(It.IsAny<string>(), property.Name, property.PropertyType)).ReturnsAsync(customField);
        }

        // Act
        _customFieldOnBoardManager.Setup();

        // Assert
        foreach (var property in properties)
        {
            _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.AddCustomFieldToBoard(It.IsAny<string>(), property.Name, property.PropertyType), Times.Once);
        }

        Assert.Equal(properties.Length, _customFieldOnBoardManager.CustomFieldsOnBoard.Count);

        foreach (var customField in customFieldsOnBoard)
        {
            Assert.Contains(customField, _customFieldOnBoardManager.CustomFieldsOnBoard);
        }
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Setup_AllCustomFieldsOnBoardExist_DoNotCreateCustomFields()
    {
        // Arrange
        var properties = typeof(Tender).GetProperties();
        var customFieldsOnBoard = CustomFieldOnBoardHelper.GenerateCustomFields(properties, true, true);

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetCustomFieldsOnBoard(It.IsAny<string>())).ReturnsAsync(customFieldsOnBoard);

        // Act
        _customFieldOnBoardManager.Setup();

        // Assert
        foreach (var property in properties)
        {
            _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.AddCustomFieldToBoard(It.IsAny<string>(), property.Name, property.PropertyType), Times.Never);
        }

        Assert.Equal(properties.Length, _customFieldOnBoardManager.CustomFieldsOnBoard.Count);

        foreach (var customField in customFieldsOnBoard)
        {
            Assert.Contains(customField, _customFieldOnBoardManager.CustomFieldsOnBoard);
        }
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Setup_CustomFieldsOnBoardExistWithMissingIds_ThrowException()
    {
        // Arrange
        var properties = typeof(Tender).GetProperties();
        var customFieldsOnBoard = CustomFieldOnBoardHelper.GenerateCustomFields(properties, false, true);

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetCustomFieldsOnBoard(It.IsAny<string>())).ReturnsAsync(customFieldsOnBoard);

        // Assert
        var exception = Assert.Throws<Exception>(Action);
        Assert.Equal($"Error getting custom field id for custom field with name: {properties.First().Name}", exception.Message);
        return;

        // Act
        void Action() => _customFieldOnBoardManager.Setup();
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Setup_CreateCustomFieldsOnBoardReturnsWithMissingIds_ThrowException()
    {
        // Arrange
        var properties = typeof(Tender).GetProperties();
        var customFieldsOnBoard = new List<CustomField>();

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetCustomFieldsOnBoard(It.IsAny<string>())).ReturnsAsync(customFieldsOnBoard);

        foreach (var property in properties)
        {
            var customField = CustomFieldOnBoardHelper.GenerateCustomField(property, false, true);

            _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.AddCustomFieldToBoard(It.IsAny<string>(), property.Name, property.PropertyType)).ReturnsAsync(customField);
        }

        // Assert
        var exception = Assert.Throws<Exception>(Action);
        Assert.Equal($"Error creating custom field on board with name: {properties.First().Name}", exception.Message);
        return;

        // Act
        void Action() => _customFieldOnBoardManager.Setup();
    }

    [Fact, Trait("Category", "UnitTests")]
    public void SetUp_Should_Throw_AppSettingsException_When_No_BoardId()
    {
        // Arrange
        _configurationMock.Setup(c => c["Trello:BoardId"]).Returns(string.Empty);

        // Assert
        var exception = Assert.Throws<AppSettingsException>(Action);
        Assert.Equal("Error getting board ID from configuration.", exception.Message);
        return;

        // Act
        void Action() => _ = new CustomFieldOnBoardManager(_configurationMock.Object, _trelloDotNetWrapperMock.Object);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Setup_SomeCustomFieldsOnBoardExist_CreateMissingCustomFields()
    {
        // Arrange
        var properties = typeof(Tender).GetProperties();
        var firstHalfProperties = properties.Take(properties.Length / 2).ToArray();
        var secondHalfProperties = properties.Skip(properties.Length / 2).ToArray();
        var customFieldsOnBoard = CustomFieldOnBoardHelper.GenerateCustomFields(firstHalfProperties, true, true);

        _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.GetCustomFieldsOnBoard(It.IsAny<string>())).ReturnsAsync(customFieldsOnBoard);

        foreach (var property in secondHalfProperties)
        {
            var customField = CustomFieldOnBoardHelper.GenerateCustomField(property, true, true);

            _trelloDotNetWrapperMock.Setup(trelloDotNetWrapper => trelloDotNetWrapper.AddCustomFieldToBoard(It.IsAny<string>(), property.Name, property.PropertyType)).ReturnsAsync(customField);
        }

        // Act
        _customFieldOnBoardManager.Setup();

        // Assert
        foreach (var property in firstHalfProperties)
        {
            _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.AddCustomFieldToBoard(It.IsAny<string>(), property.Name, property.PropertyType), Times.Never);
        }

        foreach (var property in secondHalfProperties)
        {
            _trelloDotNetWrapperMock.Verify(trelloDotNetWrapper => trelloDotNetWrapper.AddCustomFieldToBoard(It.IsAny<string>(), property.Name, property.PropertyType), Times.Once);
        }

        Assert.Equal(properties.Length, _customFieldOnBoardManager.CustomFieldsOnBoard.Count);

        foreach (var customField in customFieldsOnBoard)
        {
            Assert.Contains(customField, _customFieldOnBoardManager.CustomFieldsOnBoard);
        }
    }
}