using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Interfaces;

public interface ITrelloDotNetWrapper
{
    /// <summary>
    /// Get the lists on a board.
    /// </summary>
    /// <param name="boardId">The ID of the board.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of lists.</returns>
    Task<List<List>?> GetListsOnBoard(string boardId);

    /// <summary>
    /// Get the custom fields on a board.
    /// </summary>
    /// <param name="boardId">The ID of the board.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of custom fields.</returns>
    Task<List<CustomField>?> GetCustomFieldsOnBoard(string boardId);

    /// <summary>
    /// Add a new list to a board.
    /// </summary>
    /// <param name="boardId">The ID of the board.</param>
    /// <param name="listName">The name of the list.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added list.</returns>
    Task<List?> AddList(string boardId, string listName);

    /// <summary>
    /// Add a new custom field to a board.
    /// </summary>
    /// <param name="boardId">The ID of the board.</param>
    /// <param name="customFieldName">The name of the custom field.</param>
    /// <param name="type">The type of the custom field.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added custom field.</returns>
    Task<CustomField?> AddCustomFieldToBoard(string boardId, string customFieldName, Type type);

    /// <summary>
    /// Add a new card.
    /// </summary>
    /// <param name="card">The card to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added card.</returns>
    Task<Card> AddCard(Card card);

    /// <summary>
    /// Get a card by its ID.
    /// </summary>
    /// <param name="cardId">The ID of the card.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the card.</returns>
    Task<Card> GetCard(string cardId);

    /// <summary>
    /// Update a card.
    /// </summary>
    /// <param name="card">The card to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated card.</returns>
    Task<Card> UpdateCard(Card card);

    /// <summary>
    /// Update the value of a custom field on a card.
    /// </summary>
    /// <param name="cardId">The ID of the card.</param>
    /// <param name="customField">The custom field to update.</param>
    /// <param name="newValue">The new value of the custom field.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateCustomFieldValueOnCard(string cardId, CustomField customField, string newValue);

    /// <summary>
    /// Update the values of multiple custom fields on a card.
    /// </summary>
    /// <param name="cardId">The ID of the card.</param>
    /// <param name="customFieldItem">The list of custom field items to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateCustomFieldsValueOnCard(string cardId, List<Domain.Models.CustomFields.CustomFieldItem> customFieldItem);

    /// <summary>
    /// Search for a card on a board.
    /// </summary>
    /// <param name="boardId">The ID of the board.</param>
    /// <param name="searchTerm">The search term.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the found card.</returns>
    Task<Card?> SearchOnCard(string boardId, string searchTerm);

    /// <summary>
    /// Search for a card on a board using a specific search term.
    /// </summary>
    /// <param name="boardId">The ID of the board.</param>
    /// <param name="searchTerm">The search term.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the search result.</returns>
    Task<SearchViaBoardResult?> SearchOnCardViaBoard(string boardId, string searchTerm);
}
