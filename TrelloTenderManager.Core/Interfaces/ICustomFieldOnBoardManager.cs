using TrelloDotNet.Model;

namespace TrelloTenderManager.Core.Interfaces;

public interface ICustomFieldOnBoardManager
{
    HashSet<CustomField> CustomFieldsOnBoard { get; }
    void Setup();
}