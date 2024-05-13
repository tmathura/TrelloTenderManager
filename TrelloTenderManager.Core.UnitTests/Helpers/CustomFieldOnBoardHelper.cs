using System.Reflection;
using TrelloDotNet.Model;

namespace TrelloTenderManager.Core.UnitTests.Helpers;

public static class CustomFieldOnBoardHelper
{
    public static List<CustomField> GenerateCustomFields(PropertyInfo[] properties, bool includeId, bool includeName)
    {
        var customFieldsOnBoard = new List<CustomField>();

        foreach (var property in properties)
        {
            var customField = GenerateCustomField(property, includeId, includeName);

            customFieldsOnBoard.Add(customField);
        }

        return customFieldsOnBoard;
    }

    public static CustomField GenerateCustomField(PropertyInfo property, bool includeId, bool includeName)
    {
        var customField = new CustomField();

        if (includeId)
        {
            var idPropertyInfo = customField.GetType().GetProperty(nameof(CustomField.Id), BindingFlags.Public | BindingFlags.Instance);
            idPropertyInfo?.SetValue(customField, property.Name);
        }

        if (includeName)
        {
            var namePropertyInfo = customField.GetType().GetProperty(nameof(CustomField.Name), BindingFlags.Public | BindingFlags.Instance);
            namePropertyInfo?.SetValue(customField, property.Name);
        }

        return customField;
    }
}