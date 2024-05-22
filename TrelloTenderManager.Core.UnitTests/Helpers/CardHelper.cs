using System.Reflection;
using TrelloDotNet.Model;
using TrelloTenderManager.Core.Managers.Implementations;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.UnitTests.Helpers;

/// <summary>
/// Helper class for generating Trello cards.
/// </summary>
public static class CardHelper
{
    /// <summary>
    /// Generates a Trello card based on the provided tender and card ID.
    /// </summary>
    /// <param name="tender">The tender object.</param>
    /// <param name="cardId">The ID of the card.</param>
    /// <returns>The generated Trello card.</returns>
    public static Card GenerateCard(Tender tender, string cardId)
    {
        var card = new Card("ListId", tender.Name, CardManager.GetCardDescription(tender));

        var idPropertyInfo = card.GetType().GetProperty(nameof(Card.Id), BindingFlags.Public | BindingFlags.Instance);
        idPropertyInfo?.SetValue(card, cardId);

        return card;
    }
}
