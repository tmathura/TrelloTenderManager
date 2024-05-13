using System.Reflection;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Implementations;

public class CustomFieldManager(ITrelloDotNetWrapper trelloDotNetWrapper, ICustomFieldOnBoardManager customFieldOnBoardManager) : ICustomFieldManager
{
    private readonly PropertyInfo[] _properties = typeof(Tender).GetProperties();

    public async Task UpdateCustomFieldsOnCard(Tender tender, Card card)
    {
        foreach (var property in _properties)
        {
            try
            {
                var value = property.GetValue(tender);
                var customField = customFieldOnBoardManager.CustomFieldsOnBoard.First(customField => customField.Name == property.Name);

                var valueAsString = GetPropertyValueAsString(property, value);

                if (!string.IsNullOrWhiteSpace(valueAsString))
                {
                    await trelloDotNetWrapper.UpdateCustomFieldValueOnCard(card.Id, customField, valueAsString);
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"Error updating custom field {property.Name} on card {card.Id}", exception);
            }
        }
    }

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