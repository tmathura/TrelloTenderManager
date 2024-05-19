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
    /// Queues the content of a CSV file for processing.
    /// </summary>
    /// <param name="fileContent">The content of the CSV file.</param>
    Task QueueFromCsv(string fileContent);

    /// <summary>
    /// Populates Trello cards from the content of a CSV file.
    /// </summary>
    /// <param name="fileContent">The content of the CSV file.</param>
    /// <returns><see cref="ProcessFromCsvResult"/></returns>
    Task<ProcessFromCsvResult> ProcessFromCsv(string fileContent);

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