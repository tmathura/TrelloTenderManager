using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Interfaces;

public interface ICustomFieldManager
{
    /// <summary>
    /// Updates the custom fields on a Trello card based on the properties of a Tender object.
    /// </summary>
    /// <param name="tender">The Tender object containing the property values.</param>
    /// <param name="card">The Trello card to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateCustomFieldsOnCard(Tender tender, Card card);
}