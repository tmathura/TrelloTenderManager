using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TrelloDotNet;
using TrelloDotNet.Model;
using TrelloDotNet.Model.Search;
using TrelloTenderManager.Core.Wrappers.Interfaces;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Wrappers.Implementations;

/// <summary>
/// Provides an implementation of the <see cref="ITrelloDotNetWrapper"/> interface using the TrelloDotNet library.
/// </summary>
public class TrelloDotNetWrapper : ITrelloDotNetWrapper
{
    private readonly TrelloClient _trelloClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrelloDotNetWrapper"/> class.
    /// </summary>
    /// <param name="configuration">The configuration object.</param>
    public TrelloDotNetWrapper(IConfiguration configuration)
    {
        var apiKey = configuration["Trello:ApiKey"];
        var token = configuration["Trello:Token"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new Exception("Error getting API key from configuration.");
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new Exception("Error getting Token from configuration.");
        }

        _trelloClient = new TrelloClient(apiKey, token);
    }

    /// <inheritdoc />
    public async Task<List<List>?> GetListsOnBoard(string boardId)
    {
        List<List>? listsOnBoard = await _trelloClient.GetListsOnBoardAsync(boardId);

        return listsOnBoard;
    }

    /// <inheritdoc />
    public async Task<List<CustomField>?> GetCustomFieldsOnBoard(string boardId)
    {
        List<CustomField>? customFieldsOnBoard = await _trelloClient.GetCustomFieldsOnBoardAsync(boardId);

        return customFieldsOnBoard;
    }

    /// <inheritdoc />
    public async Task<List?> AddList(string boardId, string listName)
    {
        var list = new List(listName, boardId);
        var listOnBoard = await _trelloClient.AddListAsync(list);

        return listOnBoard;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<Card> AddCard(Card card)
    {
        var cardResult = await _trelloClient.AddCardAsync(card);

        return cardResult;
    }

    /// <inheritdoc />
    public async Task<Card> GetCard(string cardId)
    {
        var cardResult = await _trelloClient.GetCardAsync(cardId);

        return cardResult;
    }

    /// <inheritdoc />
    public async Task<Card> UpdateCard(Card card)
    {
        var cardResult = await _trelloClient.UpdateCardAsync(card);

        return cardResult;
    }

    /// <inheritdoc />
    public async Task UpdateCustomFieldValueOnCard(string cardId, CustomField customField, string newValue)
    {
        await _trelloClient.UpdateCustomFieldValueOnCardAsync(cardId, customField, newValue);
    }

    /// <inheritdoc />
    public async Task UpdateCustomFieldsValueOnCard(string cardId, List<Domain.Models.CustomFields.CustomFieldItem> customFieldItem)
    {
        var payload = new
        {
            customFieldItems = customFieldItem
        };

        var jsonPayload = JsonConvert.SerializeObject(payload);

        await _trelloClient.PutAsync($"cards/{cardId}/customFields", jsonPayload);
    }

    /// <inheritdoc />
    public async Task<Card?> SearchOnCard(string boardId, string searchTerm)
    {
        var searchRequest = new SearchRequest(searchTerm, partialSearch: false)
        {
            BoardFilter = new SearchRequestBoardFilter(boardId),
            SearchCards = true,
            SearchBoards = false,
            SearchOrganizations = false,
            CardLimit = 5,
            CardFields = new SearchRequestCardFields("desc")
        };

        var searchResult = await _trelloClient.SearchAsync(searchRequest);

        return searchResult.Cards.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<SearchViaBoardResult?> SearchOnCardViaBoard(string boardId, string searchTerm)
    {
        SearchViaBoardResult? searchViaBoardResult = null;

        var queryParameters = new QueryParameter[]
        {
                new("fields", "id,name,desc"),
        };

        var searchViaBoardResults = await _trelloClient.GetAsync<List<SearchViaBoardResult>>($"boards/{boardId}/cards", queryParameters);

        if (searchViaBoardResults.Count > 0)
        {
            searchViaBoardResult = searchViaBoardResults.FirstOrDefault(viaBoardResult => !string.IsNullOrWhiteSpace(viaBoardResult.Desc) && viaBoardResult.Desc.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        return searchViaBoardResult;
    }
}
