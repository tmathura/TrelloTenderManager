using System.Reflection;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Implementations;

public class CustomFieldOnBoardManager(ITrelloDotNetWrapper trelloDotNetWrapper, string boardId) : ICustomFieldOnBoardManager
{
    public HashSet<CustomField> CustomFieldsOnBoard { get; } = [];
    
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