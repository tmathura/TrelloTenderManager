using TrelloDotNet.Model;

namespace TrelloTenderManager.Core.Interfaces;

public interface ICustomFieldOnBoardManager
{
    /// <summary>
    /// Gets the custom fields on the board.
    /// </summary>
    HashSet<CustomField> CustomFieldsOnBoard { get; }

    /// <summary>
    /// Sets up the custom fields on the board.
    /// </summary>
    void Setup();
}