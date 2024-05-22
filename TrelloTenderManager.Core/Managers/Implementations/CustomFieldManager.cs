using System.Reflection;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Managers.Interfaces;
using TrelloTenderManager.Core.Wrappers.Interfaces;
using TrelloTenderManager.Domain.Models;
using TrelloTenderManager.Domain.Models.CustomFields;

namespace TrelloTenderManager.Core.Managers.Implementations;

/// <summary>
/// Manages the custom fields on a Trello card based on the properties of a Tender object.
/// </summary>
/// <param name="trelloDotNetWrapper">The TrelloDotNetWrapper instance.</param>
/// <param name="customFieldOnBoardManager">The CustomFieldOnBoardManager instance.</param>
public class CustomFieldManager(ITrelloDotNetWrapper trelloDotNetWrapper, ICustomFieldOnBoardManager customFieldOnBoardManager) : ICustomFieldManager
{
    private readonly PropertyInfo[] _properties = typeof(Tender).GetProperties();

    /// <inheritdoc />
    public async Task UpdateCustomFieldsOnCard(Tender tender, Card card)
    {
        var customFieldItems = new List<Domain.Models.CustomFields.CustomFieldItem>();

        foreach (var property in _properties)
        {
            try
            {
                var value = property.GetValue(tender);
                var customField = customFieldOnBoardManager.CustomFieldsOnBoard.First(customField => customField.Name == property.Name);

                var valueAsString = GetPropertyValueAsString(property, value);

                if (string.IsNullOrWhiteSpace(valueAsString)) continue;

                var propertyType = property.PropertyType;

                CustomFieldValue customFieldValue = propertyType switch
                {
                    _ when propertyType == typeof(bool) || propertyType == typeof(bool?) => new CustomFieldValueChecked { Checked = valueAsString },
                    _ when propertyType == typeof(decimal) || propertyType == typeof(decimal?) => new CustomFieldValueNumber { Number = valueAsString },
                    _ when propertyType == typeof(DateTime) || propertyType == typeof(DateTime?) => new CustomFieldValueDate { Date = valueAsString },
                    _ => new CustomFieldValueText { Text = valueAsString }
                };

                var customFieldItem = new Domain.Models.CustomFields.CustomFieldItem
                {
                    IdCustomField = customField.Id,
                    Value = customFieldValue
                };

                customFieldItems.Add(customFieldItem);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error building custom field {property.Name} on card {card.Id}", exception);
            }
        }

        await trelloDotNetWrapper.UpdateCustomFieldsValueOnCard(card.Id, customFieldItems);
    }

    /// <summary>
    /// Converts the property value to a string representation.
    /// </summary>
    /// <param name="property">The PropertyInfo object representing the property.</param>
    /// <param name="value">The value of the property.</param>
    /// <returns>The string representation of the property value.</returns>
    public static string? GetPropertyValueAsString(PropertyInfo property, object? value)
    {
        string? valueAsString = null;

        if (property.PropertyType == typeof(DateTime?))
        {
            var dateTime = (DateTime?)value;

            if (dateTime.HasValue)
            {
                valueAsString = dateTime.Value.ToString("u");
            }
        }
        else if (property.PropertyType == typeof(bool?))
        {
            var dateTime = (bool?)value;
            valueAsString = dateTime?.ToString().ToLower();
        }
        else
        {
            valueAsString = value?.ToString();
        }

        return valueAsString;
    }
}
