using Microsoft.Extensions.Configuration;
using System.Reflection;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Managers.Interfaces;
using TrelloTenderManager.Core.Wrappers.Interfaces;
using TrelloTenderManager.Domain.Exceptions;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Managers.Implementations;

/// <summary>
/// Represents a manager for custom fields on a Trello board.
/// </summary>
public class CustomFieldOnBoardManager : ICustomFieldOnBoardManager
{
    /// <summary>
    /// The TrelloDotNetWrapper instance.
    /// </summary>
    private readonly ITrelloDotNetWrapper _trelloDotNetWrapper;

    /// <summary>
    /// The ID of the Trello board.
    /// </summary>
    private static string _boardId = string.Empty;

    /// <inheritdoc />
    public HashSet<CustomField> CustomFieldsOnBoard { get; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFieldOnBoardManager"/> class.
    /// </summary>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="trelloDotNetWrapper">The TrelloDotNetWrapper instance.</param>
    /// <exception cref="AppSettingsException">Thrown when an error occurs getting the board ID from the configuration.</exception>
    public CustomFieldOnBoardManager(IConfiguration configuration, ITrelloDotNetWrapper trelloDotNetWrapper)
    {
        _trelloDotNetWrapper = trelloDotNetWrapper;
        var boardId = configuration["Trello:BoardId"];

        if (string.IsNullOrWhiteSpace(boardId))
        {
            throw new AppSettingsException("Error getting board ID from configuration.");
        }

        _boardId = boardId;
    }

    /// <inheritdoc />
    public void Setup()
    {
        var customFieldsOnBoard = _trelloDotNetWrapper.GetCustomFieldsOnBoard(_boardId).Result;
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
            var customField = _trelloDotNetWrapper.AddCustomFieldToBoard(_boardId, property.Name, property.PropertyType).Result;

            if (string.IsNullOrWhiteSpace(customField?.Id))
            {
                throw new Exception($"Error creating custom field on board with name: {property.Name}");
            }

            CustomFieldsOnBoard.Add(customField);
        }
    }
}
