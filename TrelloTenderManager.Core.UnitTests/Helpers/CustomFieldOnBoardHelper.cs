using System.Reflection;
using TrelloDotNet.Model;

namespace TrelloTenderManager.Core.UnitTests.Helpers;

/// <summary>
/// Helper class for generating custom fields on a board.
/// </summary>
public static class CustomFieldOnBoardHelper
{
    /// <summary>
    /// Generates a list of custom fields based on the given properties.
    /// </summary>
    /// <param name="properties">The properties to generate custom fields from.</param>
    /// <param name="includeId">Indicates whether to include the Id property in the generated custom fields.</param>
    /// <param name="includeName">Indicates whether to include the Name property in the generated custom fields.</param>
    /// <returns>A list of custom fields.</returns>
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

    /// <summary>
    /// Generates a custom field based on the given property.
    /// </summary>
    /// <param name="property">The property to generate the custom field from.</param>
    /// <param name="includeId">Indicates whether to include the Id property in the generated custom field.</param>
    /// <param name="includeName">Indicates whether to include the Name property in the generated custom field.</param>
    /// <returns>A custom field.</returns>
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
