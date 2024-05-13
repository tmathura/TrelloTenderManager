using TrelloDotNet;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;

namespace TrelloTenderManager.Core.Implementations;

public class TrelloDotNetWrapper(string apiKey, string token) : ITrelloDotNetWrapper
{
    private readonly TrelloClient _trelloClient = new(apiKey, token);

    public async Task<List<List>?> GetListsOnBoard(string boardId)
    {
        List<List>? listsOnBoard = await _trelloClient.GetListsOnBoardAsync(boardId);

        return listsOnBoard;
    }

    public async Task<List<CustomField>?> GetCustomFieldsOnBoard(string boardId)
    {
        List<CustomField>? customFieldsOnBoard = await _trelloClient.GetCustomFieldsOnBoardAsync(boardId);

        return customFieldsOnBoard;
    }

    public async Task<List?> AddList(string boardId, string listName)
    {
        var list = new List(listName, boardId);
        var listOnBoard = await _trelloClient.AddListAsync(list);

        return listOnBoard;
    }

    public async Task<CustomField?> AddCustomFieldToBoard(string boardId, string customFieldName, Type propertyType)
    {
        var customFieldType = propertyType switch
        {
            not null when propertyType == typeof(bool) || propertyType == typeof(bool?) => "checkbox",
            not null when propertyType == typeof(decimal) || propertyType == typeof(decimal?) => "number",
            not null when propertyType == typeof(DateTime) || propertyType == typeof(DateTime?) => "date",
            _ => "text"
        };

        var queryParameters = new QueryParameter[]
        {
            new("idModel", boardId),
            new("modelType", "board"),
            new("name", customFieldName),
            new("type", customFieldType),
            new("pos", "bottom"),
            new("display_cardFront", "true")
        };

        var customField = await _trelloClient.PostAsync<CustomField>("customFields", queryParameters);

        return customField;
    }

    public async Task<Card> AddCard(Card card)
    {
        var newCard = await _trelloClient.AddCardAsync(card);

        return newCard;
    }

    public async Task UpdateCustomFieldValueOnCard(string cardId, CustomField customField, bool newValue)
    {
        await _trelloClient.UpdateCustomFieldValueOnCardAsync(cardId, customField, newValue);
    }
}