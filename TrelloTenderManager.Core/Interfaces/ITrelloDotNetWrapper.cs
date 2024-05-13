using TrelloDotNet.Model;

namespace TrelloTenderManager.Core.Interfaces;

public interface ITrelloDotNetWrapper
{
    Task<List<List>?> GetListsOnBoard(string boardId);
    Task<List<CustomField>?> GetCustomFieldsOnBoard(string boardId);
    Task<List?> AddList(string boardId, string listName);
    Task<CustomField?> AddCustomFieldToBoard(string boardId, string customFieldName, Type type);
    Task<Card> AddCard(Card card);
    Task<Card> UpdateCard(Card card);
    Task UpdateCustomFieldValueOnCard(string cardId, CustomField customField, string newValue);
    Task<Card?> SearchOnCard(string boardId, string searchTerm);
}