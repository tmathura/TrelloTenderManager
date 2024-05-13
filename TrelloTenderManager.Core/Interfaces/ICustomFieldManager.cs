using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Interfaces;

public interface ICustomFieldManager
{
    Task UpdateCustomFieldsOnCard(Tender tender, Card card);
}