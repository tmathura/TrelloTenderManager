using System.Reflection;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Implementations;

/// <summary>
/// Manages the custom fields on a Trello board.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CustomFieldOnBoardManager"/> class.
/// </remarks>
/// <param name="trelloDotNetWrapper">The TrelloDotNetWrapper instance.</param>
/// <param name="boardId">The ID of the Trello board.</param>
public class CustomFieldOnBoardManager(ITrelloDotNetWrapper trelloDotNetWrapper, string boardId) : ICustomFieldOnBoardManager
{
    /// <inheritdoc />
    public HashSet<CustomField> CustomFieldsOnBoard { get; } = [];

    /// <inheritdoc />
    public void Setup()
    {
        var customFieldsOnBoard = trelloDotNetWrapper.GetCustomFieldsOnBoard(boardId).Result;
        var properties = typeof(Tender).GetProperties();

        if (customFieldsOnBoard is null || customFieldsOnBoard.Count == 0)
        {
            CreateCustomFieldsOnBoard(properties);
        }
        else
        {
            var propertiesToCreateFrom = properties.ToHashSet();

            foreach (var property in properties)
            {
                var customFieldOnBoard = customFieldsOnBoard.FirstOrDefault(customField => customField.Name == property.Name);

                if (customFieldOnBoard is not null)
                {
                    if (string.IsNullOrWhiteSpace(customFieldOnBoard.Id))
                    {
                        throw new Exception($"Error getting custom field id for custom field with name: {property.Name}");
                    }

                    CustomFieldsOnBoard.Add(customFieldOnBoard);
                    propertiesToCreateFrom.Remove(property);
                }
            }

            CreateCustomFieldsOnBoard(propertiesToCreateFrom);
        }
    }

    /// <summary>
    /// Creates the custom fields on the board.
    /// </summary>
    /// <param name="properties">The properties to create custom fields from.</param>
    private void CreateCustomFieldsOnBoard(IEnumerable<PropertyInfo> properties)
    {
        foreach (var property in properties)
        {
            var customField = trelloDotNetWrapper.AddCustomFieldToBoard(boardId, property.Name, property.PropertyType).Result;

            if (string.IsNullOrWhiteSpace(customField?.Id))
            {
                throw new Exception($"Error creating custom field on board with name: {property.Name}");
            }

            CustomFieldsOnBoard.Add(customField);
        }
    }
}
