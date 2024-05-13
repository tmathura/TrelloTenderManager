using TrelloDotNet.Model;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Interfaces;

public interface ICardManager
{
    /// <summary>
    /// Creates a new Trello card for the specified tender.
    /// </summary>
    /// <param name="tender">The tender for which to create the card.</param>
    /// <returns>The created card.</returns>
    Task<Card> Create(Tender tender);

    /// <summary>
    /// Updates the specified Trello card with the details from the specified tender.
    /// </summary>
    /// <param name="card">The card to update.</param>
    /// <param name="tender">The tender with the updated details.</param>
    Task Update(Card card, Tender tender);

    /// <summary>
    /// Checks if a Trello card exists for the specified tender.
    /// </summary>
    /// <param name="tender">The tender to check.</param>
    /// <returns>The existing card, if found; otherwise, null.</returns>
    Task<Card?> Exists(Tender tender);
}